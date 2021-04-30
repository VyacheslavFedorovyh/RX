using System;
using System.Text.RegularExpressions;

namespace Regular_expressions
{
    class Program
    {

        static void Main()
        {
            // String.Split Метод

            String name = "вх. 0976-исц от 13.03.2019 ОАО \"Уралгипротранс проект\"";
            String[] s = name.Split(' ');

           
            String nomber = s[1];
            //DateTime datereg;
            //if (DateTime.TryParse(s[3], out ))
            DateTime datereg = DateTime.Parse(s[3]);

            
            String corrspodent = null;
            for (int i = 4; i < s.Length; i++) 
                corrspodent = corrspodent + " " + s[i];               
                        

            Console.WriteLine("Номер: " + nomber);
            Console.WriteLine("Дата: " + datereg);
            Console.WriteLine("Корреспондент: " + corrspodent.Replace("\"", ""));

            Console.WriteLine("/////");




            //Regex regexn = new Regex(@"\s(\S+)");
            //foreach (Match matchn in regexn.Matches(name))
            //{
            //    Console.WriteLine(matchn.Groups[1].Value);
            //}
            //Console.WriteLine("/////");




            ////Допустим в исходной строке нужно найти все числа,
            ////соответствующие стоимости продукта
            //string input = "Добро пожаловать в наш магазин, вот наши цены: " +
            //        "1 кг. яблок - 20 руб. " +
            //        "2 кг. апельсинов - 30 руб. " +
            //        "0.5 кг. орехов - 50 руб.";

            //string pattern = @"\b(\d+\W?руб)";
            //Regex regex = new Regex(pattern);

            //// Получаем совпадения в экземпляре класса Match
            //Match match = regex.Match(input);

            //// отображаем все совпадения
            //while (match.Success)
            //{
            //    // Т.к. мы выделили в шаблоне одну группу (одни круглые скобки),
            //    // ссылаемся на найденное значение через свойство Groups класса Match
            //    Console.WriteLine(match.Groups[1].Value);

            //    // Переходим к следующему совпадению
            //    match = match.NextMatch();
            //}

        }
    }
}
