using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;

namespace CloudSonos.Models
{
    public class CCorreocs
    {
        Boolean estado = true;
        String merror;
        public CCorreocs(String destinatario, String usuario, String mensaje)
        {
            MailMessage correo = new MailMessage();
            SmtpClient protocolo = new SmtpClient();

            correo.To.Add("cloud.sonos@gmail.com");
            correo.From = new MailAddress("cloud.sonos@gmail.com", "CloudSonos", System.Text.Encoding.UTF8);
            correo.Subject = "Sugerencias";
            correo.SubjectEncoding = System.Text.Encoding.UTF8;
            correo.Body = "Correo enviado por: " + destinatario + "(" + usuario + ")" + " " + mensaje;
            correo.BodyEncoding = System.Text.Encoding.UTF8;
            correo.IsBodyHtml = false;

            protocolo.Credentials = new System.Net.NetworkCredential("cloud.sonos@gmail.com", "Cloudsonos25");
            protocolo.Port = 587;
            protocolo.Host = "smtp.gmail.com";
            protocolo.EnableSsl = true;
            try
            {
                protocolo.Send(correo);
            }
            catch (SmtpException ex)
            {
                estado = false;
                merror = ex.Message.ToString();
            }
        }


        public CCorreocs(String destinatario)
        {
            MailMessage correo = new MailMessage();
            SmtpClient protocolo = new SmtpClient();

            correo.To.Add(destinatario);
            correo.From = new MailAddress("cloud.sonos@gmail.com", "CloudSonos", System.Text.Encoding.UTF8);
            correo.Subject = "Bienvenido";
            correo.SubjectEncoding = System.Text.Encoding.UTF8;
            correo.Body = "Gracias por unirte a CloudSonos, Bienvenido";
            correo.BodyEncoding = System.Text.Encoding.UTF8;
            correo.IsBodyHtml = false;

            protocolo.Credentials = new System.Net.NetworkCredential("cloud.sonos@gmail.com", "Cloudsonos25");
            protocolo.Port = 587;
            protocolo.Host = "smtp.gmail.com";
            protocolo.EnableSsl = true;
            try
            {
                protocolo.Send(correo);
            }
            catch (SmtpException ex)
            {
                estado = false;
                merror = ex.Message.ToString();
            }
        }

    }
}