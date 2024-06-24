using Employee_Portal.Model;

namespace Employee_Portal.Core
{
    public interface IEmployee
    {
        public  Task<AddingEmployResponse> AddingEmployee(EmployeeMaster employeeMaster);
        public Task<IEnumerable<EmployeeMaster>> GettingAllEmployees();
        public Task<DeletingEmployResponse> DeleteEmploy(int Id);
        public Task<UpdatedEmployeResponse>Update(EmployeeMaster employeeMaster);
        public  Task<IEnumerable<Country>> Countries();
        public  Task<IEnumerable<State>> States(int id);
        public  Task<IEnumerable<City>> Cities(int id);
        public Task<EmployeeMaster> GetById(int id);
        public Task<IEnumerable<EmployeeFetch>> FetchEmployees();



    }
}
