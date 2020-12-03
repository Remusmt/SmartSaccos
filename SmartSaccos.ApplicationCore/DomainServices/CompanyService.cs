using SmartSaccos.ApplicationCore.Interfaces;
using SmartSaccos.Domains.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartSaccos.ApplicationCore.DomainServices
{
    public class CompanyService
    {
        private readonly IRepository<Company> companyRepository;
        private readonly IRepository<Country> countryRepository;
        private readonly IRepository<Currency> currencyRepository;
        private readonly IRepository<CompanyDefaults> companyDefaultsRepository;
        public CompanyService(
            IRepository<Company> companyRepo,
            IRepository<Country> countryRepo,
            IRepository<Currency> currencyRepo,
            IRepository<CompanyDefaults> companyDefaultsRepo
            )
        {
            companyRepository = companyRepo;
            countryRepository = countryRepo;
            currencyRepository = currencyRepo;
            companyDefaultsRepository = companyDefaultsRepo;
        }

        public async Task<Company> Register(
            string companyName,
            int countryId,
            DateTimeOffset createdOn
            )
        {
            Country country = await countryRepository.GetByIdAsync(countryId);

            if (country == null) throw new Exception("Invalid country.");
            if (!country.CurrencyId.HasValue) throw new Exception("Selected country has no currency defined.");

            Company company = await companyRepository
                .FirstOrDefaultAsync(e => e.CompanyName == companyName);

            if (company != null) return company;

            company = new Company
            {
                CompanyName = companyName,
                CountryId = country.Id,
                CurrencyId = country.CurrencyId.Value,
                CreatedOn = createdOn
            };

            // Define defaults
            CompanyDefaults companyDefaults = new CompanyDefaults
            {
                CompanyId = company.Id,
                DefaultCurrency = company.CurrencyId,
                AllowPostingToParentAccount = true,
                CreatedOn = createdOn,
                UseAccountNumbers = false
            };
            companyDefaultsRepository.Add(companyDefaults);
            companyRepository.Add(company);
            await companyRepository.SaveChangesAsync();

            return company;
        }

        public async Task<Company> GetCompanyAsync(int companyId)
        {
            return await companyRepository.GetByIdAsync(companyId);
        }

        public async Task<CompanyDefaults> GetCompanyDefaultsAsync(int companyId)
        {
            return await companyDefaultsRepository
                .FirstOrDefaultAsync(e => e.CompanyId == companyId);
        }

        public async Task<List<Country>> GetCountriesAsync()
        {
            return await countryRepository.ListAllAsync();
        }

        public async Task<List<Currency>> GetCurrenciesAsync()
        {
            return await currencyRepository.ListAllAsync();
        }
    }
}
