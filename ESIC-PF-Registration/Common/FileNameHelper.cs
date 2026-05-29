using System.Text.RegularExpressions;

namespace ESIC_PF_Registration.Common
{

    public static class FileNameHelper
    {
        // Matches: 20260526_160642  (yyyyMMdd_HHmmss)
        private static readonly Regex DateTimePrefixRegex =
            new Regex(@"^\d{8}_\d{6}_", RegexOptions.Compiled);

        // Matches a 32-char hex guid-like token (your example: 2686d576e5544277a077891eb6d2ead1)
        private static readonly Regex Hex32Regex =
            new Regex(@"^[a-fA-F0-9]{32}$", RegexOptions.Compiled);

        // Matches canonical GUID with hyphens (just in case)
        private static readonly Regex GuidRegex =
            new Regex(@"^[a-fA-F0-9]{8}\-[a-fA-F0-9]{4}\-[a-fA-F0-9]{4}\-[a-fA-F0-9]{4}\-[a-fA-F0-9]{12}$",
                RegexOptions.Compiled);

        /// <summary>
        /// Creates a friendly display file name from a stored file path.
        /// </summary>
        /// <param name="filePath">Stored relative/absolute path</param>
        /// <param name="fallbackBaseName">Optional label like "Aadhaar" / "PAN" / "Photo". If provided, output will be "Label.ext".</param>
        /// <param name="defaultIfNoExt">If file has no extension, this is used.</param>
        public static string CleanDisplayName(string? filePath, string? fallbackBaseName = null, string defaultIfNoExt = "Document")
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return string.IsNullOrWhiteSpace(fallbackBaseName) ? defaultIfNoExt : fallbackBaseName;

            var file = Path.GetFileName(filePath);                // 20260526_160642_xxx.pdf
            var ext = Path.GetExtension(file);                    // .pdf (or .jpg)
            var name = Path.GetFileNameWithoutExtension(file);    // 20260526_160642_xxx

            // If you already KNOW the label (Aadhaar/PAN/etc.), use it with same extension.
            if (!string.IsNullOrWhiteSpace(fallbackBaseName))
            {
                return fallbackBaseName.Trim() + (string.IsNullOrWhiteSpace(ext) ? "" : ext);
            }

            // Otherwise, try to clean typical generated patterns:

            // 1) Remove leading datetime prefix: 20260526_160642_
            name = DateTimePrefixRegex.Replace(name, "");

            // 2) Split by '_' and remove guid-like chunks
            var parts = name.Split('_', StringSplitOptions.RemoveEmptyEntries).ToList();
            parts.RemoveAll(p => Hex32Regex.IsMatch(p) || GuidRegex.IsMatch(p));

            // 3) If nothing meaningful remains, fallback
            var cleanedBase = parts.Count > 0 ? string.Join("_", parts) : defaultIfNoExt;

            return cleanedBase + (string.IsNullOrWhiteSpace(ext) ? "" : ext);
        }
    }
}
