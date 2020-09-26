using MDL_BASE.Models;
using MDL_BASE.Models.Base;
using MDLX_CORE.ComponentCore.Entities;
using MDL_PFEP.Model;
using MDL_PFEP.Model.PFEP;
using MDL_PFEP.Repo.PFEP;
using MDL_PRD.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDL_PPRPA.Tests
{
    [TestClass]
    public class Test1
    {
        
        [TestMethod]
        public void TestBatch20_BatchNew()
        {
            ProductionOrder order = new ProductionOrder();

            order.StartDate = new DateTime(2018, 2, 1, 10, 0, 0);
            order.EndDate = new DateTime(2018, 2, 1, 10, 1, 40);
            order.QtyPlanned = 100;
            order.QtyRemain = 100;
            
            ProdOrderBatcher ob = new ProdOrderBatcher(order);
            List<Prodorder20> list = ob.BatchNew();

            Assert.AreEqual(5, list.Count);
        }

        [TestMethod]
        public void TestBatch20_BatchExisting_InProd()
        {
            ProductionOrder order = new ProductionOrder();
            order.StartDate = new DateTime(2018, 2, 1, 10, 0, 0);
            order.EndDate = new DateTime(2018, 2, 1, 10, 1, 40);
            order.QtyPlanned = 100;
            order.QtyRemain = 100;
            ProdOrderBatcher ob = new ProdOrderBatcher(order);
            List<Prodorder20> list = ob.BatchNew();

            ProductionOrder order1 = new ProductionOrder();
            order1.StartDate = new DateTime(2018, 2, 1, 10, 0, 0);
            order1.EndDate = new DateTime(2018, 2, 1, 10, 1, 40);
            order1.QtyPlanned = 100;
            order1.QtyRemain = 65;
            ProdOrderBatcher ob1 = new ProdOrderBatcher(order1);
            List<Prodorder20> list1 = ob1.BatchExisting(list);

            Assert.AreEqual(5, list.Count);
        }

        [TestMethod]
        public void TestBatch20_BatchExisting_QtyIncr()
        {
            ProductionOrder order = new ProductionOrder();
            order.StartDate = new DateTime(2018, 2, 1, 10, 0, 0);
            order.EndDate = new DateTime(2018, 2, 1, 10, 1, 40);
            order.QtyPlanned = 100;
            order.QtyRemain = 100;
            ProdOrderBatcher ob = new ProdOrderBatcher(order);
            List<Prodorder20> list = ob.BatchNew();

            ProductionOrder order1 = new ProductionOrder();
            order1.StartDate = new DateTime(2018, 2, 1, 10, 0, 0);
            order1.EndDate = new DateTime(2018, 2, 1, 10, 2, 10);
            order1.QtyPlanned = 130;
            order1.QtyRemain = 130;
            ProdOrderBatcher ob1 = new ProdOrderBatcher(order1);
            List<Prodorder20> list1 = ob1.BatchExisting(list);

            Assert.AreEqual(7, list1.Count);
        }
    }
}