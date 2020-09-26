using MDL_iLOGIS.ComponentConfig.Mappers;
using MDL_ONEPROD.ComponentMes.Etities;
using MDL_ONEPROD.Model.Scheduling;
using MDL_ONEPROD.Repo;
using MDLX_MASTERDATA.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_ONEPROD.ComponentMes.Models
{
    public class TreantJsModel
    {
        private UnitOfWorkOneProdMes uow;

        public TreantJsModel(UnitOfWorkOneProdMes uowork)
        {
            this.uow = uowork;
        }

        
        public TreantJsNode GetParentNode(int productionLogId)
        {
            ProductionLog temp =  uow.ProductionLogRepo.GetById(productionLogId);
            return MapProductionLogToTreantJsNode(temp);
        }
        public TreantJsNode ConnectParentWithChilds(int productionLogId, List<TreantJsNode> childs)
        {
            ProductionLog temp = uow.ProductionLogRepo.GetById(productionLogId);
            TreantJsNode parent = MapProductionLogToTreantJsNode(temp);

            foreach (var child in childs)
            {
                parent.children.Add(child);
            }
            return parent;
        }
        public List<TreantJsNode> GetChildNodes(int productionLogId)
        {
            List<TreantJsNode> treantNodes = new List<TreantJsNode>();
            List<ProductionLogTraceability> prodLogTraceabilityList;
            prodLogTraceabilityList = uow.ProductionLogTraceabilityRepo.GetByParentId(productionLogId).ToList();

            foreach (var prodLogTrace in prodLogTraceabilityList)
            {
                if (prodLogTrace.Child != null)
                {
                    TreantJsNode node = MapProductionLogToTreantJsNode(prodLogTrace.Child);
                    treantNodes.Add(node);
                }
                else
                {
                    TreantJsNode node = MapProductionLogTraceabilityToTreantJsNode(prodLogTrace);
                    treantNodes.Add(node);
                }
            }
            return treantNodes;
        }
        public List<TreantJsNode> GetParentNodes(int productionLogId)
        {
            //return uow.ProductionLogRepo.GetTreantParentsByPrLogId(productionLogId);
            return uow.ProductionLogRepo.GetList().Where(x => x.Id == productionLogId).ToListTreant<TreantJsNode>();
        }

        private TreantJsNode MapProductionLogToTreantJsNode(ProductionLog child)
        {
            if (child != null)
            {
                return ProductionLogToTreantJsNodeMapper.Map(child);
            }
            else
            {
                return null;
            }
        }
        private TreantJsNode MapProductionLogTraceabilityToTreantJsNode(ProductionLogTraceability prodLogTrace)
        {
            if (prodLogTrace != null)
            {
                Item item = uow.ItemRepo.GetByCode(prodLogTrace.ItemCode);

                return new TreantJsNode()
                {
                    text = new TreantJsNodeText()
                    {
                        name = prodLogTrace.ItemCode,
                        desc = item != null ? item.Name : "Nazwa ?",
                        title = "",
                        datetime = prodLogTrace.ProductionDate.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                        serialNumber = prodLogTrace.SerialNumber,
                        prodLogId = "",
                        declaredQty = "",
                        workOrderTotalQty = "",
                        //picture = "<i class='far fa-image'></i>",
                    },
                    image = "<i class='far fa-image'></i>"
                };
            }
            else
            {
                return null;
            }
        }
    }
}