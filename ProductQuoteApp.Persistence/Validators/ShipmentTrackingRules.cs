using System;
using System.ComponentModel.DataAnnotations;

namespace ProductQuoteApp.Persistence
{
    public static class ShipmentTrackingRules
    {
        public static ValidationResult ValidateShipmentTracking(ShipmentTracking shipmentTracking, ValidationContext validationContext)
        {
            if (shipmentTracking == null)
                return null;

            //Prioridades de Completadas
            if (shipmentTracking.CustomerOrderCompleted && !shipmentTracking.QuotedCompleted)
            {
                return new ValidationResult("La Etapa 'Orden de Compra' no puede estar completada si aun no está completada la Etapa anterior");
            }
            if (shipmentTracking.ApprovedCompleted && !shipmentTracking.CustomerOrderCompleted)
            {
                return new ValidationResult("La Etapa 'Aprobado' no puede estar completada si aun no está completada la Etapa anterior");
            }
            if (shipmentTracking.InProductionEnabled && shipmentTracking.InProductionCompleted && !shipmentTracking.ApprovedCompleted)
            {
                return new ValidationResult("La Etapa 'En Px' no puede estar completada si aun no está completada la Etapa anterior");

            }
            if (shipmentTracking.ETDEnabled && shipmentTracking.ETDCompleted)
            {
                if (shipmentTracking.InProductionEnabled && !shipmentTracking.InProductionCompleted)
                {
                    return new ValidationResult("La Etapa 'Embarcado ETD' no puede estar completada si aun no está completada la Etapa anterior");
                }
                if (!shipmentTracking.InProductionEnabled && !shipmentTracking.ApprovedCompleted)
                {
                    return new ValidationResult("La Etapa 'Embarcado ETD' no puede estar completada si aun no está completada la Etapa anterior");
                }
            }
            if (shipmentTracking.ETAEnabled && shipmentTracking.ETACompleted)
            {
                if (shipmentTracking.ETDEnabled && !shipmentTracking.ETDCompleted)
                {
                    return new ValidationResult("La Etapa 'Puerto ETA' no puede estar completada si aun no está completada la Etapa anterior");
                }
                if (shipmentTracking.InProductionEnabled && !shipmentTracking.InProductionCompleted)
                {
                    return new ValidationResult("La Etapa 'Puerto ETA' no puede estar completada si aun no está completada la Etapa anterior");
                }
            }
            if (shipmentTracking.NationalizedEnabled && shipmentTracking.NationalizedCompleted)
            {
                if (shipmentTracking.ETAEnabled && !shipmentTracking.ETACompleted)
                {
                    return new ValidationResult("La Etapa 'Nacionalizado' no puede estar completada si aun no está completada la Etapa anterior");
                }
                if (shipmentTracking.ETDEnabled && !shipmentTracking.ETDCompleted)
                {
                    return new ValidationResult("La Etapa 'Nacionalizado' no puede estar completada si aun no está completada la Etapa anterior");
                }
                if (shipmentTracking.InProductionEnabled && !shipmentTracking.InProductionCompleted)
                {
                    return new ValidationResult("La Etapa 'Nacionalizado' no puede estar completada si aun no está completada la Etapa anterior");
                }
            }
            if (shipmentTracking.DeliveredCompleted)
            {
                if (shipmentTracking.NationalizedEnabled && !shipmentTracking.NationalizedCompleted)
                {
                    return new ValidationResult("La Etapa 'Entregado' no puede estar completada si aun no está completada la Etapa anterior");
                }
                if (shipmentTracking.ETAEnabled && !shipmentTracking.ETACompleted)
                {
                    return new ValidationResult("La Etapa 'Entregado' no puede estar completada si aun no está completada la Etapa anterior");
                }
                if (shipmentTracking.ETDEnabled && !shipmentTracking.ETDCompleted)
                {
                    return new ValidationResult("La Etapa 'Entregado' no puede estar completada si aun no está completada la Etapa anterior");
                }
                if (shipmentTracking.InProductionEnabled && !shipmentTracking.InProductionCompleted)
                {
                    return new ValidationResult("La Etapa 'Entregado' no puede estar completada si aun no está completada la Etapa anterior");
                }
            }



            //Fechas Estimadas
            if (shipmentTracking.ApprovedEstimatedDate < shipmentTracking.CustomerOrderRealDate)
            {
                return new ValidationResult("La Fecha Estimada de 'Aprobado' no puede ser menor a la Fecha Real de la Etapa anterior");
            }
            if (shipmentTracking.InProductionEnabled)
            {
                if (shipmentTracking.InProductionEstimatedDate < shipmentTracking.ApprovedRealDate)
                {
                    return new ValidationResult("La Fecha Estimada de 'En Px' no puede ser menor a la Fecha Real de la Etapa anterior");
                }
                if (shipmentTracking.InProductionEstimatedDate < shipmentTracking.ApprovedEstimatedDate)
                {
                    return new ValidationResult("La Fecha Estimada de 'En Px' no puede ser menor a la Fecha Estimada de la Etapa anterior");
                }
            }
            if (shipmentTracking.ETDEnabled)
            {
                if ((shipmentTracking.ETDEstimatedDate < shipmentTracking.InProductionRealDate) && (shipmentTracking.InProductionEnabled))
                {
                    return new ValidationResult("La Fecha Estimada de 'Embarcado ETD' no puede ser menor a la Fecha Real de la Etapa anterior");
                }
                if (shipmentTracking.ETDEstimatedDate < shipmentTracking.ApprovedRealDate)
                {
                    return new ValidationResult("La Fecha Estimada de 'Embarcado ETD' no puede ser menor a la Fecha Real de la Etapa anterior");
                }
                if (shipmentTracking.ETDEstimatedDate < shipmentTracking.ApprovedEstimatedDate)
                {
                    return new ValidationResult("La Fecha Estimada de 'Embarcado ETD' no puede ser menor a la Fecha Estimada de la Etapa anterior");
                }
            }
            if (shipmentTracking.ETAEnabled)
            {
                if ((shipmentTracking.ETAEstimatedDate < shipmentTracking.ETDRealDate) && (shipmentTracking.ETDEnabled))
                {
                    return new ValidationResult("La Fecha Estimada de 'Puerto ETA' no puede ser menor a la Fecha Real de la Etapa anterior");
                }
                if ((shipmentTracking.ETAEstimatedDate < shipmentTracking.ETDEstimatedDate) && (shipmentTracking.ETDEnabled))
                {
                    return new ValidationResult("La Fecha Estimada de 'Puerto ETA' no puede ser menor a la Fecha Estimada de la Etapa anterior");
                }
                if ((shipmentTracking.ETAEstimatedDate < shipmentTracking.InProductionRealDate) && (shipmentTracking.InProductionEnabled))
                {
                    return new ValidationResult("La Fecha Estimada de 'Puerto ETA' no puede ser menor a la Fecha Real de la Etapa anterior");
                }
                if ((shipmentTracking.ETAEstimatedDate < shipmentTracking.InProductionEstimatedDate) && (shipmentTracking.InProductionEnabled))
                {
                    return new ValidationResult("La Fecha Estimada de 'Puerto ETA' no puede ser menor a la Fecha Estimada de la Etapa anterior");
                }
                if (shipmentTracking.ETAEstimatedDate < shipmentTracking.ApprovedRealDate)
                {
                    return new ValidationResult("La Fecha Estimada de 'Puerto ETA' no puede ser menor a la Fecha Real de la Etapa anterior");
                }
                if (shipmentTracking.ETAEstimatedDate < shipmentTracking.ApprovedEstimatedDate)
                {
                    return new ValidationResult("La Fecha Estimada de 'Puerto ETA' no puede ser menor a la Fecha Estimada de la Etapa anterior");
                }

            }
            if (shipmentTracking.NationalizedEnabled)
            {
                if ((shipmentTracking.NationalizedEstimatedDate < shipmentTracking.ETARealDate) && (shipmentTracking.ETAEnabled))
                {
                    return new ValidationResult("La Fecha Estimada de 'Nacionalizado' no puede ser menor a la Fecha Real de la Etapa anterior");
                }
                if ((shipmentTracking.NationalizedEstimatedDate < shipmentTracking.ETAEstimatedDate) && (shipmentTracking.ETAEnabled))
                {
                    return new ValidationResult("La Fecha Estimada de 'Nacionalizado' no puede ser menor a la Fecha Estimada de la Etapa anterior");
                }
                if ((shipmentTracking.NationalizedEstimatedDate < shipmentTracking.ETDRealDate) && (shipmentTracking.ETDEnabled))
                {
                    return new ValidationResult("La Fecha Estimada de 'Nacionalizado' no puede ser menor a la Fecha Real de la Etapa anterior");
                }
                if ((shipmentTracking.NationalizedEstimatedDate < shipmentTracking.ETDEstimatedDate) && (shipmentTracking.ETDEnabled))
                {
                    return new ValidationResult("La Fecha Estimada de 'Nacionalizado' no puede ser menor a la Fecha Estimada de la Etapa anterior");
                }
                if ((shipmentTracking.NationalizedEstimatedDate < shipmentTracking.InProductionRealDate) && (shipmentTracking.InProductionEnabled))
                {
                    return new ValidationResult("La Fecha Estimada de 'Nacionalizado' no puede ser menor a la Fecha Real de la Etapa anterior");
                }
                if ((shipmentTracking.NationalizedEstimatedDate < shipmentTracking.InProductionEstimatedDate) && (shipmentTracking.InProductionEnabled))
                {
                    return new ValidationResult("La Fecha Estimada de 'Nacionalizado' no puede ser menor a la Fecha Estimada de la Etapa anterior");
                }
            }//acaaaaaaaaaaaaa
            if ((shipmentTracking.DeliveredEstimatedDate < shipmentTracking.NationalizedRealDate) && (shipmentTracking.NationalizedEnabled))
            {
                return new ValidationResult("La Fecha Estimada de 'Entregado' no puede ser menor a la Fecha Real de la Etapa anterior");
            }
            if ((shipmentTracking.DeliveredEstimatedDate < shipmentTracking.NationalizedEstimatedDate) && (shipmentTracking.NationalizedEnabled))
            {
                return new ValidationResult("La Fecha Estimada de 'Entregado' no puede ser menor a la Fecha Estimada de la Etapa anterior");
            }
            if ((shipmentTracking.DeliveredEstimatedDate < shipmentTracking.ETARealDate) && (shipmentTracking.ETAEnabled))
            {
                return new ValidationResult("La Fecha Estimada de 'Entregado' no puede ser menor a la Fecha Real de la Etapa anterior");
            }
            if ((shipmentTracking.DeliveredEstimatedDate < shipmentTracking.ETAEstimatedDate) && (shipmentTracking.ETAEnabled))
            {
                return new ValidationResult("La Fecha Estimada de 'Entregado' no puede ser menor a la Fecha Estimada de la Etapa anterior");
            }
            if ((shipmentTracking.DeliveredEstimatedDate < shipmentTracking.ETDRealDate) && (shipmentTracking.ETDEnabled))
            {
                return new ValidationResult("La Fecha Estimada de 'Entregado' no puede ser menor a la Fecha Real de la Etapa anterior");
            }
            if ((shipmentTracking.DeliveredEstimatedDate < shipmentTracking.ETDEstimatedDate) && (shipmentTracking.ETDEnabled))
            {
                return new ValidationResult("La Fecha Estimada de 'Entregado' no puede ser menor a la Fecha Estimada de la Etapa anterior");
            }
            if ((shipmentTracking.DeliveredEstimatedDate < shipmentTracking.InProductionRealDate) && (shipmentTracking.InProductionEnabled))
            {
                return new ValidationResult("La Fecha Estimada de 'Entregado' no puede ser menor a la Fecha Real de la Etapa anterior");
            }
            if ((shipmentTracking.DeliveredEstimatedDate < shipmentTracking.InProductionEstimatedDate) && (shipmentTracking.InProductionEnabled))
            {
                return new ValidationResult("La Fecha Estimada de 'Entregado' no puede ser menor a la Fecha Estimada de la Etapa anterior");
            }




            //Fechas Reales
            if (shipmentTracking.ApprovedRealDate < shipmentTracking.CustomerOrderRealDate && shipmentTracking.ApprovedRealDate != null)
            {
                return new ValidationResult("La Fecha Real de 'Aprobado' no puede ser menor a la Fecha Real de la la Etapa anterior");
            }
            if (
                (!shipmentTracking.ETDEnabled) && //Faltaba
                (shipmentTracking.InProductionEnabled) && 
                (shipmentTracking.InProductionRealDate != null) &&
                (shipmentTracking.InProductionRealDate < shipmentTracking.ApprovedRealDate || shipmentTracking.ApprovedRealDate == null)
                )
            {
                return new ValidationResult("La Fecha Real de 'En Px' no puede ser menor a la Fecha Real de la Etapa anterior");
            }

            if ((shipmentTracking.ETDEnabled) && (shipmentTracking.ETDRealDate != null))
            {
                if ((shipmentTracking.ETDRealDate < shipmentTracking.InProductionRealDate) && (shipmentTracking.InProductionEnabled))
                {
                    return new ValidationResult("La Fecha Real de 'Embarcado ETD' no puede ser menor a la Fecha Real de la Etapa anterior");
                }
                if (shipmentTracking.InProductionRealDate < shipmentTracking.ApprovedRealDate || shipmentTracking.ApprovedRealDate == null)
                {
                    return new ValidationResult("La Fecha Real de 'Embarcado ETD' no puede ser menor a la Fecha Real de la Etapa anterior");
                }
            }
            if ((shipmentTracking.ETAEnabled) && (shipmentTracking.ETARealDate != null))
            {
                if ((shipmentTracking.ETARealDate < shipmentTracking.ETDRealDate) && (shipmentTracking.ETDEnabled))
                {
                    return new ValidationResult("La Fecha Real de 'Puerto ETA' no puede ser menor a la Fecha Real de la Etapa anterior");
                }
                if ((shipmentTracking.ETARealDate < shipmentTracking.InProductionRealDate) && (shipmentTracking.InProductionEnabled))
                {
                    return new ValidationResult("La Fecha Real de 'Puerto ETA' no puede ser menor a la Fecha Real de la Etapa anterior");
                }
            }
            if ((shipmentTracking.NationalizedEnabled) && (shipmentTracking.NationalizedRealDate != null))
            {
                if ((shipmentTracking.NationalizedRealDate < shipmentTracking.ETARealDate) && (shipmentTracking.ETAEnabled))
                {
                    return new ValidationResult("La Fecha Real de 'Nacionalizado' no puede ser menor a la Fecha Real de la Etapa anterior");
                }
                if ((shipmentTracking.NationalizedRealDate < shipmentTracking.ETDRealDate) && (shipmentTracking.ETDEnabled))
                {
                    return new ValidationResult("La Fecha Real de 'Nacionalizado' no puede ser menor a la Fecha Real de la Etapa anterior");
                }
                if ((shipmentTracking.NationalizedRealDate < shipmentTracking.InProductionRealDate) && (shipmentTracking.InProductionEnabled))
                {
                    return new ValidationResult("La Fecha Real de 'Nacionalizado' no puede ser menor a la Fecha Real de la Etapa anterior");
                }

            }
            if (shipmentTracking.DeliveredRealDate != null)
            {
                if ((shipmentTracking.DeliveredRealDate < shipmentTracking.NationalizedRealDate) && (shipmentTracking.NationalizedEnabled))
                {
                    return new ValidationResult("La Fecha Real de 'Entregado' no puede ser menor a la Fecha Real de la Etapa anterior");
                }
                if ((shipmentTracking.DeliveredRealDate < shipmentTracking.ETARealDate) && (shipmentTracking.ETAEnabled))
                {
                    return new ValidationResult("La Fecha Real de 'Entregado' no puede ser menor a la Fecha Real de la Etapa anterior");
                }
                if ((shipmentTracking.DeliveredRealDate < shipmentTracking.ETDRealDate) && (shipmentTracking.ETDEnabled))
                {
                    return new ValidationResult("La Fecha Real de 'Entregado' no puede ser menor a la Fecha Real de la Etapa anterior");
                }
                if ((shipmentTracking.DeliveredRealDate < shipmentTracking.InProductionRealDate) && (shipmentTracking.InProductionEnabled))
                {
                    return new ValidationResult("La Fecha Real de 'Entregado' no puede ser menor a la Fecha Real de la Etapa anterior");
                }
            }


            return null;
        }
    }
}
