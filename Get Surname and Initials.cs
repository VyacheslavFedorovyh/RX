using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConsoleApp1
{
    class Program
        {
            /// <summary>
            /// Получить фамилию и инициалы в формате Фамилия И.О.
            /// </summary>
            /// <param name="fio">ФИО</param>
            /// <returns>Фамилия и инициалы.</returns>
            [Public]
            public static string GetShortNameGD(string fio)
            {
            try
            {
                string[] str = fio.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (str.Length != 3) throw new ArgumentException("ФИО задано в неверно формате");
                return string.Format("{0} {1}. {2}.", str[0], str[1][0], str[2][0]);
            }
            catch (ArgumentException e)
            {
                return fio = null;
            }
        }
    }
}
