using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductQuoteApp.Persistence;
using ProductQuoteApp.Library;
using System.ComponentModel;

namespace ProductQuoteApp.Services
{
    public class TransportServices: ITransportServices
    {
        private IGeographicAreaTransportTypeRepository geographicAreaTransportTypeRepository = null;

        public TransportServices(IGeographicAreaTransportTypeRepository geographicAreaTransportTypeRepo)
        {
            geographicAreaTransportTypeRepository = geographicAreaTransportTypeRepo;
        }

        private IEnumerable<GeographicAreaTransportType> GetAllTransportTypesOrdered(int geographicAreaID, int freightTypeID)
        {
            List<GeographicAreaTransportType> listGAT = geographicAreaTransportTypeRepository.FindGeographicAreaTransportTypesByGeographicArea(geographicAreaID);

            return listGAT.OrderByDescending(o => o.TransportType.PositionQuantityTo).Where(o => o.TransportType.PositionQuantityTo > 0).Where(o => o.TransportType.FreightTypeID == freightTypeID);
        }

        private GeographicAreaTransportType SearchTransportType(IEnumerable<GeographicAreaTransportType> transportTypesList, int quantity, ref int rest)
        {
            //buscamos desde el mas grande donde entraria la entrega

            foreach (var transporte in transportTypesList)
            {
                if ((quantity >= transporte.TransportType.PositionQuantityFrom))
                {
                    rest = quantity - transporte.TransportType.PositionQuantityTo;
                    return transporte;
                }
            }

            if (transportTypesList.Any())
            {
                rest = quantity - transportTypesList.Last().TransportType.PositionQuantityTo;
                return transportTypesList.Last();

            }

            return null;
        }

        private decimal GetDeliveryCost(IEnumerable<GeographicAreaTransportType> transportTypeList)
        {
            decimal ret = 0;

            foreach (var item in transportTypeList)
            {
                ret = ret + item.FreightCost;
            }

            return ret;
        }

        private string CaptionFreightCalculation(string leyendaInicial, List<GeographicAreaTransportType> fletesEntrega)
        {
            ///(Semi26+Semi4)*4 + (Semi26) = $XXX
            ///Para toda la operación de 140 Tn costarían $XXX
            ///Y por Tn serían $XXX / TC / 140
            ///Entrega Individual[$150] = ((((Playo 4 Posiciones[$20])*7) + PickUp[$10])*1)*6
            ///Entrega Individual[$150] = ((Playo 4 Posiciones[$20])*7) + PickUp[$10])*1)*6
            StringBuilder stb = new StringBuilder();
            StringBuilder stb2 = new StringBuilder();

            var fleteAgrup = fletesEntrega.GroupBy(f => f.TransportType.Description);

            foreach (var item in fleteAgrup)
            {
                if (stb.Length > 0)
                    stb.Append(" + ");
                stb.Append("[[").Append(item.Key.ToString()).Append("]*").Append(item.Count().ToString()).Append("]");
            }

            if (stb.Length > 0)
            {
                if (leyendaInicial == "Entrega Resto")
                {
                    stb2.Append("] + ").Append(leyendaInicial).Append(" = [[").Append(stb.ToString()).Append("]]");
                }
                else
                {
                    stb2.Append(leyendaInicial).Append(" = ").Append(stb.ToString());
                }
            }

            return stb2.ToString();
        }

        public decimal GetFleteFRMT_PorItem(ProductQuote productQuote, Product product)
        {
            ///transportCost: Transporte: Según tabla en función a la cantidad mínima por entrega. Si retira cliente es cero. Luego será transporte 
            ///chico, chasis o semi en función a la cantidad (tiene que poder cambiarse la cantidad porque depende del costo de transporte tbn 
            ///(a veces te conviene más dos chasis que un semi). Siempre que el envase del producto sea granel liquido o solido, el transporte será el 
            ///correspondiente (y tendrá ese costo)
            ///
            ///Compra Total = 140 ton
            ///Entrega Min = 30 ton
            ///Son 4 entregas de 30 ton + 1 entrega de 20 ton
            ///Para las primeras 4 entregas se utilizan en cada una: un Semi de 26 posiciones y uno de 4 posiciones
            ///y para la ultima entrega un Semi de 26 posiciones.
            ///(Semi26 + Semi4) * 4 + (Semi26) = 43.200 ?????

            GeographicAreaTransportType transportType = null;
            List<GeographicAreaTransportType> transportTypeList = new List<GeographicAreaTransportType>();
            List<GeographicAreaTransportType> transportTypeList2 = new List<GeographicAreaTransportType>();
            int totalQty = productQuote.QuantityOpenPurchaseOrder;
            int deliveryQty = productQuote.MinimumQuantityDelivery;
            int deliveryEntire = totalQty / deliveryQty;
            int deliveryRest = totalQty % deliveryQty;
            int rest = deliveryQty;
            int rest2 = deliveryRest;

            //Recuperamos los Tipos de Transporte filtrados por Tipo de Carga y por Area Geografica
            IEnumerable<GeographicAreaTransportType> transportList = GetAllTransportTypesOrdered(productQuote.GeographicAreaID, product.FreightTypeID);

            //Si no existen tranportes, devuelve costo 0 (cero), la leyenda interna 'NO SE ENCONTRO FLETE' y la leyenda al usuario: 'Esta cotización no incluye el costo del flete, el cual deberá considerarse por separado'
            if (!transportList.Any())
            {
                productQuote.LeyendaCalculoCostoTransporte = "NO SE ENCONTRO FLETE";
                //productQuote.Observations = "Esta cotización no incluye el costo del flete, el cual deberá considerarse por separado.";
                return 0;
            }

            //Si encontro fletes, va sumando fletes por cada MinimumQuantityDelivery hasta completar la OC
            while (rest > 0)
            {
                transportType = SearchTransportType(transportList, deliveryQty, ref rest);
                if ((transportType == null) && (transportTypeList.Count == 0))
                {
                    productQuote.LeyendaCalculoCostoTransporte = "NO SE ENCONTRO FLETE";
                    //productQuote.Observations = "Esta cotización no incluye el costo del flete, el cual deberá considerarse por separado.";
                    return 0;
                }
                transportTypeList.Add(transportType);
                deliveryQty = rest;
            }

            decimal deliveryCost = GetDeliveryCost(transportTypeList);
            decimal deliveryCostAll = deliveryCost * deliveryEntire;


            //falta calcular el rest de totalQty / deliveryQty
            while (rest2 > 0)
            {
                transportType = SearchTransportType(transportList, deliveryRest, ref rest2);
                transportTypeList2.Add(transportType);
                deliveryRest = rest2;
            }

            decimal costodeliveryRest = GetDeliveryCost(transportTypeList2);
            decimal totalResult = deliveryCostAll + costodeliveryRest;

            productQuote.LeyendaCalculoCostoTransporte = CaptionFreightCalculation("Entrega Individual", transportTypeList) + "]*" + deliveryEntire + CaptionFreightCalculation("Entrega rest", transportTypeList2);

            return totalResult;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && geographicAreaTransportTypeRepository != null)
            {
                geographicAreaTransportTypeRepository.Dispose();
                geographicAreaTransportTypeRepository = null;
            }
        }

    }
}
