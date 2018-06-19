using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SportsStore.Domain.Concrete
{


    public class EmailOrderProcessor : IOrderProcessor
    {
        public void ProcessOrder(Cart cart, ShippingDetails shippingInfo)
        {
            
            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("sklepecomm@gmail.com", "sklep123");

            StringBuilder body = new StringBuilder().AppendLine("Nowe zamówienie").AppendLine("---").AppendLine("Produkty:");
            foreach (var line in cart.Lines)
            {
                var subtotal = line.Product.Price * line.Quantity;
                body.AppendFormat("{0} x {1} (wartość: {2:c}", line.Quantity, line.Product.Name, subtotal)
                    .Append(")")
                    .AppendLine();
            }
            body.AppendFormat("\nWartość całkowita: {0:c}", cart.ComputeTotalValue())
                .AppendLine("---")
                .AppendLine("Wysyłka dla:")
                .AppendLine(shippingInfo.Name)
                .AppendLine(shippingInfo.Line1)
                .AppendLine(shippingInfo.Line2 ?? "")
                .AppendLine(shippingInfo.Line3 ?? "")
                .AppendLine(shippingInfo.City)
                .AppendLine(shippingInfo.State ?? "")
                .AppendLine(shippingInfo.Country)
                .AppendLine(shippingInfo.Zip)
                .AppendLine("---");


            MailMessage mailMessage = new MailMessage(
                "sklepecomm@gmail.com", // od 
                shippingInfo.Email,
                "Otrzymano nowe zamówienie!", // temat 
                body.ToString()); // treść
           
            client.Send(mailMessage);
           
        }
    }
    }
