export interface Attachment {
  id: number;
  rootPath: string;
  contentDisposition: string;
  contentType: string;
  fileName: string;
  systemFileName: string;
  extension: string;
  length: number;
  name: string;
  fullPath: string;
}
