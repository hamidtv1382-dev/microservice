using System.Text;
using System.Text.RegularExpressions;

namespace Catalog_Service.src._01_Domain.Core.Primitives
{

    public class Slug : ValueObject
    {
        public string Value { get; }

        private Slug(string value)
        {
            Value = value;
        }

        public static Slug Create(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Slug title cannot be empty", nameof(title));

            string slug = GenerateSlug(title);
            return new Slug(slug);
        }

        public static Slug FromString(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
                throw new ArgumentException("Slug cannot be empty", nameof(slug));

            if (!IsValidSlug(slug))
                throw new ArgumentException("Invalid slug format", nameof(slug));

            return new Slug(slug);
        }

        private static string GenerateSlug(string title)
        {
            // Convert to lowercase
            string slug = title.ToLowerInvariant();

            // Transliterate Persian, Russian, and other characters to English
            slug = Transliterate(slug);

            // Remove invalid characters (allow only letters, numbers, hyphens, spaces)
            slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");

            // Convert spaces to hyphens
            slug = Regex.Replace(slug, @"\s+", "-").Trim();

            // Remove multiple hyphens
            slug = Regex.Replace(slug, @"[-]{2,}", "-");

            return slug;
        }

        private static string Transliterate(string source)
        {
            var sb = new StringBuilder();

            // Mapping for Persian and Russian characters
            // You can extend this dictionary as needed
            var charMap = new Dictionary<char, string>
            {
                // Persian
                { 'ا', "a" }, { 'آ', "aa" }, { 'ب', "b" }, { 'پ', "p" }, { 'ت', "t" }, { 'ث', "s" },
                { 'ج', "j" }, { 'چ', "ch" }, { 'ح', "h" }, { 'خ', "kh" }, { 'د', "d" }, { 'ذ', "z" },
                { 'ر', "r" }, { 'ز', "z" }, { 'ژ', "zh" }, { 'س', "s" }, { 'ش', "sh" }, { 'ص', "s" },
                { 'ض', "z" }, { 'ط', "t" }, { 'ظ', "z" }, { 'ع', "a" }, { 'غ', "gh" }, { 'ف', "f" },
                { 'ق', "gh" }, { 'ک', "k" }, { 'گ', "g" }, { 'ل', "l" }, { 'م', "m" }, { 'ن', "n" },
                { 'و', "v" }, { 'ه', "h" }, { 'ی', "y" },

                // Russian
                { 'а', "a" }, { 'б', "b" }, { 'в', "v" }, { 'г', "g" }, { 'д', "d" }, { 'е', "e" },
                { 'ё', "yo" }, { 'ж', "zh" }, { 'з', "z" }, { 'и', "i" }, { 'й', "y" }, { 'к', "k" },
                { 'л', "l" }, { 'м', "m" }, { 'н', "n" }, { 'о', "o" }, { 'п', "p" }, { 'р', "r" },
                { 'с', "s" }, { 'т', "t" }, { 'у', "u" }, { 'ф', "f" }, { 'х', "kh" }, { 'ц', "ts" },
                { 'ч', "ch" }, { 'ш', "sh" }, { 'щ', "shch" }, { 'ъ', "" }, { 'ы', "y" }, { 'ь', "" },
                { 'э', "e" }, { 'ю', "yu" }, { 'я', "ya" }
            };

            foreach (char c in source)
            {
                if (charMap.ContainsKey(c))
                {
                    sb.Append(charMap[c]);
                }
                else if (char.IsLetterOrDigit(c) || c == ' ' || c == '-')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        private static bool IsValidSlug(string slug)
        {
            return Regex.IsMatch(slug, @"^[a-z0-9]+(?:-[a-z0-9]+)*$");
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
