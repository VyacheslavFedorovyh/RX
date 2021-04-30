using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;

namespace finex.Subject.Module.RecordManagementUI.Client
{
		/// <summary>
		/// Транслитерация текста из кирилицы в латиницу
		/// </summary>
		/// <param name="text">Текст (кирилица)</param>
		/// <returns>Текст в латинице</returns>
		[Public, Remote]
		public static string TransliterateCyrillictoLatin(string text)
		{
			string[] lat_up =  {"A",  "B", "V", "G", "D", "E", "Yo", "ZH", "Z", "I", "Y", "K", "L", "M", "N", "O", "P", "R", "S", "T", "U", "F", "KH", "TS", "CH", "SH", "Shch", "",  "Y", "",  "E", "YU", "YA"};
			string[] lat_low = {"a",  "b", "v", "g", "d", "e", "yo", "zh", "z", "i", "y", "k", "l", "m", "n", "o", "p", "r", "s", "t", "u", "f", "kh", "ts", "ch", "sh", "shch", "",  "y", "",  "e", "yu", "ya"};
			string[] rus_up =  {"А",  "Б", "В", "Г", "Д", "Е", "Ё",  "Ж",  "З", "И", "Й", "К", "Л", "М", "Н", "О", "П", "Р", "С", "Т", "У", "Ф", "Х",  "Ц",  "Ч",  "Ш",  "Щ",    "Ъ", "Ы", "Ь", "Э", "Ю", "Я"};
			string[] rus_low = { "а", "б", "в", "г", "д", "е", "ё",  "ж",  "з", "и", "й", "к", "л", "м", "н", "о", "п", "р", "с", "т", "у", "ф", "х",  "ц",  "ч",  "ш",  "щ",    "ъ", "ы", "ь", "э", "ю", "я"};
			
			for (int i = 0; i <= 32; i++)
			{
				text = text.Replace(rus_up[i], lat_up[i]);
				text = text.Replace(rus_low[i], lat_low[i]);
			}

			return text;
		}
}
