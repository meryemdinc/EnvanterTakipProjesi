namespace Application.Exceptions;
public class ItemNotAvailableException : Exception
 {
        // base(message) diyerek, C#'ın orijinal Exception sınıfının constructor'ına mesajımızı gönderiyoruz.
  public ItemNotAvailableException(string message) : base(message)
   {
   }
 }
