using NUnit.Framework;
using SmartSaccos.ApplicationCore.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartSaccos.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Test1()
        {
            MessageService messageService = new MessageService();
            bool sent = await messageService.SendEmail("remusmt@gmail.com", "Muthomi", "Testing", "From sacco");
            Assert.IsTrue(sent);
        }

        [Test]
        public void HackerRankTest()
        {
            Assert.AreEqual(2, DivisibleSumPairs(4, new int[] { 1, 3, 2, 4, 2 }));
            //Assert.AreEqual(5, CrudeDivide(5, new int[] { 1, 3, 2, 6, 1, 2 }));
        }

        private int DivisibleSumPairs(int k, int[] ar)
        {
            //there can only be k compliments of k {0 to k-1}
            int[] complements = new int[k];
            int count = 0;
            foreach (int value in ar)
            {
                //reminder after division by k
                int modValue = value % k;
                //get compliment
                int compliment = (k - modValue);
                //get index, get modula of k for when the value is factor of k to zerorize
                int index = compliment % k;
                //add count by number of compliments that occured in list b4
                count += complements[index];
                complements[modValue]++;
            }
            return count;
        }

        private int ForcedPairs(int k, int[] ar)
        {
            //Create list to hold value to compliment to make factors of k
            List<int> complements = new List<int>(ar.Length);
            int count = 0;
            foreach (var value in ar)
            {
                int modValue = value % k;
                //Add value to compliment
                complements.Add(modValue);
                // get the value that compliments modValue
                int modVCompliment = k - modValue;
                // check if compliment already exists in list
                if (complements.Contains(modVCompliment))
                {
                    //A compliment exists
                    count++;
                } else if (complements.Contains(0) && modValue == 0)
                {
                    // it is a factor of k and a similar one existed b4
                    count++;
                }
            }
            return count;
        }

        private int CrudeDivide(int k, int[] ar)
        {
            int n = ar.Length;
            int returnValue = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = n - 1; j > i; j--)
                {
                    if ((ar[i] + ar[j]) % k == 0)
                    {
                        returnValue++;
                    }
                }
            }
            return returnValue;
        }
    }
}