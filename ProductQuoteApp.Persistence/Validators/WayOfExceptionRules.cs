using System;
using System.ComponentModel.DataAnnotations;

namespace ProductQuoteApp.Persistence
{
    public static class WayOfExceptionRules
    {
        public static ValidationResult ValidateWayOfException(WayOfException wayOfException, ValidationContext validationContext)
        {
            if (wayOfException == null)
                return null;

            if (wayOfException.QuantityOpenPurchaseOrder < wayOfException.DeliveryAmount)
            {
                return new ValidationResult("La Cantidad Total Orden de Compra (Kg) no puede ser menor a la Cantidad de Entregas.");
            }

            if ((wayOfException.QuantityOpenPurchaseOrder % wayOfException.DeliveryAmount) != 0)
            {
                return new ValidationResult("La Cantidad Total Orden de Compra (Kg) / Cantidad de Entregas, debe resultar en un numero entero.");
            }

            return null;

        }
    }
}
