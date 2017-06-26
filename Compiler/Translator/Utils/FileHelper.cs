using Bridge.Contract;
using Bridge.Contract.Constants;

using Object.Net.Utilities;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Bridge.Translator
{
    public class FileHelper
    {
        public string GetSymmetricFileName(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return fileName;
            }

            var extention = Path.GetExtension(fileName);
            var nameWithoutExtention = Path.GetFileNameWithoutExtension(fileName);

            var isMin = IsMin(fileName);

            if (isMin)
            {
                return StringUtils.ReplaceLastInstanceOf(fileName, Files.Extensions.AnyMin + extention, extention);
            }

            if (string.IsNullOrEmpty(extention))
            {
                return fileName;
            }

            return StringUtils.ReplaceLastInstanceOf(fileName, extention, Files.Extensions.AnyMin + extention);
        }

        public string EnsureMinifiedFileName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || IsMin(fileName))
            {
                return fileName;
            }

            return GetSymmetricFileName(fileName);
        }

        public string EnsureNonMinifiedFileName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || !IsMin(fileName))
            {
                return fileName;
            }

            return GetSymmetricFileName(fileName);
        }

        public bool IsMin(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return false;
            }

            var nameWithoutExtention = Path.GetFileNameWithoutExtension(fileName);

            if (!nameWithoutExtention.Contains("."))
            {
                // It does not have 'min' in fileName like nameWithoutExtention.min.extention
                return false;
            }

            return string.Compare(nameWithoutExtention, Files.Extensions.AnyMin, true) == 0
                || nameWithoutExtention.EndsWith(Files.Extensions.AnyMin, StringComparison.InvariantCultureIgnoreCase);
        }

        public bool IsJS(string fileName)
        {
            if (fileName == null)
            {
                return false;
            }

            return fileName.EndsWith(Files.Extensions.JS, StringComparison.InvariantCultureIgnoreCase);
        }

        public bool IsMinJS(string fileName)
        {
            return IsJS(fileName) && IsMin(fileName);
        }

        public bool IsDTS(string fileName)
        {
            if (fileName == null)
            {
                return false;
            }

            return fileName.EndsWith(Files.Extensions.DTS, StringComparison.InvariantCultureIgnoreCase);
        }

        public bool IsCSS(string fileName)
        {
            if (fileName == null)
            {
                return false;
            }

            return fileName.EndsWith(Files.Extensions.CSS, StringComparison.InvariantCultureIgnoreCase);
        }

        public TranslatorOutputType GetOutputType(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return TranslatorOutputType.None;
            }

            if (IsJS(fileName))
            {
                return TranslatorOutputType.JavaScript;
            }

            if (IsDTS(fileName))
            {
                return TranslatorOutputType.TypeScript;
            }

            if (IsCSS(fileName))
            {
                return TranslatorOutputType.StyleSheets;
            }

            return TranslatorOutputType.None;
        }

        public string CheckFileNameAndOutputType(string fileName, TranslatorOutputType outputType, bool isMinified = false)
        {
            if (outputType == TranslatorOutputType.None)
            {
                return null;
            }

            var outputTypeByFileName = GetOutputType(fileName);

            if (outputTypeByFileName == outputType)
            {
                return null;
            }

            string changeExtention = null;

            switch (outputTypeByFileName)
            {
                case TranslatorOutputType.JavaScript:
                    if (IsMinJS(fileName))
                    {
                        changeExtention = Files.Extensions.MinJS;
                    }
                    else
                    {
                        changeExtention = Files.Extensions.JS;
                    }
                    break;
                case TranslatorOutputType.TypeScript:
                    changeExtention = Files.Extensions.DTS;
                    break;
                case TranslatorOutputType.StyleSheets:
                    changeExtention = Files.Extensions.CSS;
                    break;
                default:
                    break;
            }

            if (changeExtention != null)
            {
                fileName = StringUtils.ReplaceLastInstanceOf(fileName, changeExtention, string.Empty);
            }

            if (fileName[fileName.Length - 1] == '.')
            {
                fileName = fileName.Remove(fileName.Length - 1);
            }

            switch (outputType)
            {
                case TranslatorOutputType.JavaScript:
                    if (isMinified)
                    {
                        fileName = fileName + Files.Extensions.MinJS;
                    }
                    else
                    {
                        fileName = fileName + Files.Extensions.JS;
                    }

                    return fileName;
                case TranslatorOutputType.TypeScript:
                    return fileName + Files.Extensions.DTS;
                case TranslatorOutputType.StyleSheets:
                    return fileName + Files.Extensions.CSS;
                default:
                    return null;
            }
        }

        public FileInfo CreateFileDirectory(string outputPath, string fileName)
        {
            return CreateFileDirectory(Path.Combine(outputPath, fileName));
        }

        public FileInfo CreateFileDirectory(string path)
        {
            var file = new System.IO.FileInfo(path);

            if (!file.Directory.Exists)
            {
                file.Directory.Create();
            }

            return file;
        }

        class StringUtils
        {
            public static string ReplaceLastInstanceOf(string text, string oldValue, string newValue)
            {
                if (text.IsEmpty())
                {
                    return text;
                }

                return string.Format("{0}{1}{2}", LeftOfRightmostOf(text, oldValue), newValue, RightOfRightmostOf(text, oldValue));
            }

            public static string LeftOfRightmostOf(string text, string value)
            {
                if (text.IsEmpty())
                {
                    return text;
                }

                int i = text.LastIndexOf(value, StringComparison.InvariantCultureIgnoreCase);

                if (i == -1)
                {
                    return text;
                }

                return text.Substring(0, i);
            }

            public static string RightOfRightmostOf(string text, string value)
            {
                if (text.IsEmpty())
                {
                    return text;
                }

                int i = text.LastIndexOf(value, StringComparison.InvariantCultureIgnoreCase);

                if (i == -1)
                {
                    return text;
                }

                return text.Substring(i + value.Length);
            }

        }
    }
}
