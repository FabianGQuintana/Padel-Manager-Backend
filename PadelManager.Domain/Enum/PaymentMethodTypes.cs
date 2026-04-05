using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Domain.Enum
{
    public enum PaymentMethodTypes
    {
        Cash = 1, //Efectivo
        Transfer = 2, //Transferencia
        CreditCard = 3, //Tarjeta de credito
        DebitCard = 4, //Tarjeta de debito
        DiscountEspecific = 5 //Descuento especifico (hijo del dueño,etc)
    }
}
