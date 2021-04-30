using System;
using MailKit.Net.Imap;
using MailKit.Search;
using MailKit;
using MimeKit;

namespace IntegrationXML
{
	class Program
    {
        static void Main(string[] args)
        {
            using (var client = new ImapClient())
            {
                // Для демонстрационных целей, принимать все SSL сертификаты
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                client.Connect("imap.yandex.ru", 993, true);
                client.Authenticate("IntegrationXML@yandex.ru", "1Qwerty");

                // Проверка подключения к серверу
                // Доделать потом (если нет подключения, что то делать)
                //if (client.IsConnected == true)
               // {
                    // Папка «Входящие» всегда доступна на всех серверах IMAP ...
                    var inbox = client.Inbox;
                    inbox.Open(FolderAccess.ReadWrite);

                // Ищем новые сообщения не прочитанные
                foreach (var uid in inbox.Search(SearchQuery.NotSeen))
                {
                    // Получаем не прочитанное письмо
                    var message = inbox.GetMessage(uid);
                    foreach (MimeEntity attachment in message.Attachments)
                    {
                        // Узнаем имя вложения
                        string contentname = null;

                        if (attachment.ContentDisposition.FileName != null)
                        {
                            contentname = attachment.ContentDisposition.FileName;
                        }
                        var fileName = contentname ?? attachment.ContentType.Name;

                        // Имя файла
                        Console.WriteLine(fileName);
                    }
                    // Изменение статуса письма с "не прочитанно" => "прочитано
                    inbox.AddFlags(uid, MessageFlags.Seen, true);
                }
                client.Disconnect(true);
                Console.ReadLine();
                
            }
        }
    }
}
