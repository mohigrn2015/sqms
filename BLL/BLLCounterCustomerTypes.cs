using SQMS.DAL;
using SQMS.Models.ViewModels;
using System.Data;

namespace SQMS.BLL
{
    public class BLLCounterCustomerTypes
    {
        public List<VMCounterCustomerType> GetAll()
        {
            DALCounterCustomerTypes dal = new DALCounterCustomerTypes();
            DataTable dt = dal.GetAll();
            return ObjectMappingListVM(dt);
        }

        public VMCounterCustomerType GetById(int id)
        {
            DALCounterCustomerTypes dal = new DALCounterCustomerTypes();
            DataTable dt = dal.GetById(id);
            if (dt.Rows.Count > 0)
                return ObjectMapping(dt.Rows[0]);
            else return null;
        }

        public void Create(VMCounterCustomerType counterCustType)
        {
            DALCounterCustomerTypes dal = new DALCounterCustomerTypes();
            int counter_id = dal.Insert(counterCustType);
            counterCustType.counter_id = counter_id;
        }


        public void Edit(VMCounterCustomerType counterCustType)
        {
            DALCounterCustomerTypes dal = new DALCounterCustomerTypes();
            dal.Update(counterCustType);
        }


        public void ActiveOrDeactive(VMCounterCustomerType counterCustType)
        {
            DALCounterCustomerTypes dal = new DALCounterCustomerTypes();
            dal.ActiveOrDeactive(counterCustType);
        }              

        internal VMCounterCustomerType ObjectMapping(DataRow row)
        {

            VMCounterCustomerType counterCustType = new VMCounterCustomerType();
            counterCustType.branch_id = Convert.ToInt32(row["branch_id"] == DBNull.Value ? 0 : row["branch_id"]);
            counterCustType.counter_id = Convert.ToInt32(row["counter_id"] == DBNull.Value ? 0 : row["counter_id"]);
            counterCustType.customer_type_id = Convert.ToInt32(row["customer_type_id"] == DBNull.Value ? 0 : row["customer_type_id"]);
            counterCustType.counter_customer_type_id = Convert.ToInt32(row["counter_customer_type_id"] == DBNull.Value ? 0 : row["counter_customer_type_id"]);
            counterCustType.is_active = Convert.ToInt32(row["is_active"] == DBNull.Value ? 0 : row["is_active"]);

            return counterCustType;
        }


        internal List<VMCounterCustomerType> ObjectMappingListVM(DataTable dt)
        {
            List<VMCounterCustomerType> list = new List<VMCounterCustomerType>();
            foreach (DataRow row in dt.Rows)
            {
                VMCounterCustomerType counterCustType = new VMCounterCustomerType();
                counterCustType.counter_customer_type_id = Convert.ToInt32(row["counter_customer_type_id"] == DBNull.Value ? 0 : row["counter_customer_type_id"]);
                counterCustType.branch_id = Convert.ToInt32(row["branch_id"] == DBNull.Value ? 0 : row["branch_id"]);
                counterCustType.branch_name = (row["branch_name"] == DBNull.Value ? null : row["branch_name"].ToString());
                counterCustType.counter_id = Convert.ToInt32(row["counter_id"] == DBNull.Value ? 0 : row["counter_id"]);
                counterCustType.counter_no = (row["counter_no"] == DBNull.Value ? null : row["counter_no"].ToString());
                counterCustType.branch_name = (row["branch_name"] == DBNull.Value ? null : row["branch_name"].ToString());
                counterCustType.counter_id = Convert.ToInt32(row["counter_id"] == DBNull.Value ? 0 : row["counter_id"]);
                counterCustType.customer_type_name = (row["customer_type_name"] == DBNull.Value ? null : row["customer_type_name"].ToString());

                if (dt.Columns.Contains("is_active")) counterCustType.is_active = Convert.ToInt32(row["is_active"] == DBNull.Value ? 0 : row["is_active"]);

                list.Add(counterCustType);
            }
            return list;
        }
    }
}
