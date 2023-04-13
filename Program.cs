using System;
using System.IO;
using System.Text.RegularExpressions;

class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("请输入要重命名文件的路径（输入 q 退出程序）：");
            string path = Console.ReadLine();

            if (path.ToLower() == "q")
            {
                break;
            }

            if (!Directory.Exists(path))
            {
                Console.WriteLine("输入的路径不存在，请检查后重试。");
                continue;
            }

            // 获取指定目录下的所有文件
            string[] files = Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly);

            foreach (string file in files)
            {
                string filePath = Path.GetFullPath(file);
                string fileName = Path.GetFileName(filePath);

                // 匹配包含在[]中的字符串
                MatchCollection matches = Regex.Matches(fileName, @"\[(.*?)\]");

                if (matches.Count > 0)
                {
                    string[] newParts = new string[matches.Count + 1];
                    int i = 0;

                    foreach (Match match in matches)
                    {
                        string matchStr = match.Groups[1].Value;

                        if (Regex.IsMatch(matchStr, @"^[0-9]{2}$"))
                        {
                            // 规则1：如果是"01"、"02"、"03"……一直到"99"，则将该字符串修改为"S01E01"、"S01E02"、"S01E03"……至"S01E99"
                            newParts[i++] = "S01E" + matchStr;
                        }
                        else
                        {
                            // 规则2：如果不是规则1中的字符串，则保留不变
                            newParts[i++] = matchStr;
                        }
                    }

                    // 将规则1和规则2中的部分拼接成一个新的文件名
                    string newFileName = string.Join(".", newParts, 0, i);
                    newFileName += Path.GetExtension(fileName);

                    // 重命名文件
                    string newFilePath = Path.Combine(Path.GetDirectoryName(filePath), newFileName);
                    File.Move(filePath, newFilePath);
                }
            }

            Console.WriteLine("文件重命名完成。");
        }
    }
}
