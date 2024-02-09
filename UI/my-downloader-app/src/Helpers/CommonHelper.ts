export class CommonHelper {
    public static getFileNameFromHeader(content: string): string {
        const prefix = 'filename=';
        let fileName = content.split('; ').find(name => name.startsWith(prefix));
        fileName = fileName?.substring(prefix.length);
        fileName = fileName?.startsWith('"') && fileName.endsWith('"') ? fileName.slice(1, -1) : fileName;

        return fileName ? fileName : "";
    }
}
