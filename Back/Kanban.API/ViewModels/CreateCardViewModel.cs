using Flunt.Notifications;
using Flunt.Validations;
using Kanban.API.Models;

namespace Kanban.API.ViewModels
{
    public class CreateCardViewModel : Notifiable<Notification>
    {
        public string Titulo { get; set; }
        public string Conteudo { get; set; }
        public string Lista { get; set; }

        public Card MapTo(int idCard)
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNull(Titulo, "Informe o título do Card")
                .IsGreaterThan(Titulo, 3, "O título deve conter mais de 3 caracteres"));

            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNull(Conteudo, "Informe o conteúdo do Card")
                .IsGreaterThan(Conteudo, 3, "O conteudo deve conter mais de 3 caracteres"));

            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNull(Lista, "Informe o nome da Lista que contem o Card")
                .IsGreaterThan(Lista, 3, "O nome da Lista deve conter mais de 3 caracteres"));

            return new Card(){
                Id = idCard,
                Titulo = Titulo,
                Conteudo = Conteudo,
                Lista = Lista
            };
        }
    }
}
