using Azure;
using Employee_Portal.Database;
using Employee_Portal.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Employee_Portal.Core
{
    public class EmployeeCore : IEmployee
    {
        private readonly DatabaseContext _dbContext;
        public EmployeeCore(DatabaseContext databaseContext)
        {
            _dbContext = databaseContext;
        }

        public async Task<AddingEmployResponse> AddingEmployee(EmployeeMaster employeeMaster)
        {
            AddingEmployResponse response = new AddingEmployResponse();
            var val = _dbContext.Employees.Select(x => x.Row_Id).OrderByDescending(y => y).ToList().Take(1).FirstOrDefault();
            if (val != 0)//Auto Generated Prefix of 2 Zeros
            {
                val += 1;
                employeeMaster.EmployeeCode = "00" + val;
            }
            else
            {
                employeeMaster.EmployeeCode = "001";
            }
            //Checking PanNumber Uniqueness
            var pancard = _dbContext.Employees.Where(x => x.PanNumber == employeeMaster.PanNumber).FirstOrDefault();
            if (pancard != null)
            {
                response.IsAdded = false;
                response.Message = "PanNumberAlreadyExisted";
                return response;
            }
            //Checking Passport Uniquness
            var passportcheck = _dbContext.Employees.Where(x => x.PassportNumber == employeeMaster.PassportNumber).FirstOrDefault();
            if (passportcheck != null)
            {
                response.IsAdded = false;
                response.Message = "PassPortAlreadyExisted";
                return response;
            }
            //Checking Email Uniquenss
            var emailcheck = _dbContext.Employees.Where(x => x.EmailAddress == employeeMaster.EmailAddress).FirstOrDefault();
            if (emailcheck != null)
            {
                response.IsAdded = false;
                response.Message = "EmailAlreadyExisted";
                return response;
            }
            //checking Mobile Number Uniqueness
            var mobile=_dbContext.Employees.Where(x=>x.MobileNumber==employeeMaster.MobileNumber).FirstOrDefault(); 
            if(mobile != null)
            {
                response.IsAdded = false;
                response.Message = "MobileAlreadyExisted";
                return response;
            }
            employeeMaster.CreatedDate = DateTime.Now;
            employeeMaster.IsDeleted=false;
            await _dbContext.Employees.AddAsync(employeeMaster);
            await _dbContext.SaveChangesAsync();
            response.IsAdded=true;
            response.Message = "EmployeeAddedSucessfully";
            return response;

        }

        public async Task<IEnumerable<City>> Cities(int id)
        {
            return await _dbContext.Cities.Where(x => x.stateId == id).ToListAsync();
        }

        public async Task<IEnumerable<Country>> Countries()
        {

            var list= await _dbContext.Countries.ToListAsync();
            return list;
        }

        //Deleting The Employee
        public async Task<DeletingEmployResponse> DeleteEmploy(int Id)
        {
            DeletingEmployResponse response = new DeletingEmployResponse();
            var employ =await _dbContext.Employees.FindAsync(Id);
            if (employ == null)
            {
                response.IsDeleted = false;
                response.Message = "EmployIdDoesn'tExists";
                return response;
            }
            _dbContext.Employees.Remove(employ);
            await _dbContext.SaveChangesAsync();
            response.IsDeleted=true;
            response.Message = "EmployDeletedSuccessfully";
            return response;
        }

        public async Task<IEnumerable<EmployeeFetch>> FetchEmployees()
        {
            var Countrylist = await _dbContext.Countries.ToListAsync();
            var Statelist = await _dbContext.States.ToListAsync();
            var Citylist = await _dbContext.Cities.ToListAsync();
            List<EmployeeFetch> list=new List<EmployeeFetch>();
            var allEmployees = await _dbContext.Employees.ToArrayAsync();
            foreach(var ele in allEmployees)
            {
                EmployeeFetch employeeFetch = new EmployeeFetch();
                employeeFetch.Row_Id= ele.Row_Id;
                employeeFetch.FirstName=ele.FirstName;
                employeeFetch.LastName=ele.LastName;
                employeeFetch.EmailAddress = ele.EmailAddress;
                employeeFetch.CreatedDate = ele.CreatedDate;
                employeeFetch.UpdatedDate = ele.UpdatedDate;
                employeeFetch.IsDeleted = ele.IsDeleted;
                employeeFetch.DateOfBirth = ele.DateOfBirth;
                employeeFetch.DateOfJoinee = ele.DateOfJoinee;
                employeeFetch.DeletedDate = ele.DeletedDate;
                employeeFetch.Gender = ele.Gender;
                employeeFetch.MobileNumber = ele.MobileNumber;
                employeeFetch.PanNumber = ele.PanNumber;
                employeeFetch.EmployeeCode = ele.EmployeeCode;
                employeeFetch.PassportNumber = ele.PassportNumber;
                employeeFetch.ProfileImage=ele.ProfileImage;
                employeeFetch.Imagefile=ele.Imagefile;
                employeeFetch.IsActive=ele.IsActive;
                employeeFetch.country = Countrylist.FirstOrDefault(x => x.Row_Id == ele.countryId).CountryName;
                employeeFetch.state = Statelist.FirstOrDefault(x => x.Row_Id == ele.stateId).StateName;
                employeeFetch.city = Citylist.FirstOrDefault(x => x.Row_Id == ele.cityId).CityName;
                list.Add(employeeFetch);
            }
            return list;


        }

        public async Task<EmployeeMaster> GetById(int id)
        {

          var ele= _dbContext.Employees.FirstOrDefault(x => x.Row_Id == id);
            if (ele != null)
            {
                return ele;
            }
            return null;
        }

        public async Task<IEnumerable<EmployeeMaster>> GettingAllEmployees()
        {
            var list = _dbContext.Employees.ToList();
            var emp = list.Where(x => x.Row_Id == (list.Select(y => y.Row_Id).Max(z => z))).First();
            //list is Displaying in Ascending Order
            return await _dbContext.Employees.OrderBy(x=>x.Row_Id).ToListAsync();
        }

        public async Task<IEnumerable<State>> States(int id)
        {
            return await _dbContext.States.Where(x => x.countryId == id).ToListAsync();
        }

        public async Task<UpdatedEmployeResponse> Update(EmployeeMaster ele)
        {
            UpdatedEmployeResponse updated = new UpdatedEmployeResponse();
            var employee = await _dbContext.Employees.FindAsync(ele.Row_Id);
            if (employee == null)
            {
                updated.IsUpdated = false;
                updated.Message = "Employee Not Found";
                return updated;
            }
            //Checking PanNumber Uniqueness
            var pancard = _dbContext.Employees.Where(x =>x.Row_Id!=ele.Row_Id && x.PanNumber == ele.PanNumber).FirstOrDefault();
            if (pancard != null)
            {
                updated.IsUpdated = false;
                updated.Message = "PanNumberAlreadyExisted";
                return updated;
            }
            //Checking Passport Uniquness
            var passportcheck = _dbContext.Employees.Where(x =>x.Row_Id!=ele.Row_Id && x.PassportNumber == ele.PassportNumber).FirstOrDefault();
            if (passportcheck != null)
            {
                updated.IsUpdated = false;
                updated.Message = "PassPortAlreadyExisted";
                return updated;
            }
            //Checking Email Uniquenss
            var emailcheck = _dbContext.Employees.Where(x =>x.Row_Id!=ele.Row_Id && x.EmailAddress == ele.EmailAddress).FirstOrDefault();
            if (emailcheck != null)
            {
                updated.IsUpdated = false;
                updated.Message = "EmailAlreadyExisted";
                return updated;
            }
            var mobile = _dbContext.Employees.Where(x =>x.Row_Id!=ele.Row_Id && x.MobileNumber == ele.MobileNumber).FirstOrDefault();
            if (mobile != null)
            {
                updated.IsUpdated = false;
                updated.Message = "MobileAlreadyExisted";
                return updated;
            }
            //Updating the date
            employee.FirstName = ele.FirstName;
            employee.LastName = ele.LastName;
            employee.EmailAddress = ele.EmailAddress;
            employee.UpdatedDate = DateTime.Now;//updated Date will be Assigned
            employee.IsDeleted = ele.IsDeleted;
            employee.DateOfBirth = ele.DateOfBirth;
            employee.DateOfJoinee = ele.DateOfJoinee;
            employee.Gender = ele.Gender;
            employee.countryId = ele.countryId;
            employee.stateId = ele.stateId;
            employee.cityId = ele.cityId;
            employee.ProfileImage = ele.ProfileImage;
            employee.MobileNumber = ele.MobileNumber;
            employee.PanNumber = ele.PanNumber;
            employee.PassportNumber = ele.PassportNumber;
            employee.IsActive = ele.IsActive;
            _dbContext.Employees.Update(employee);
            await _dbContext.SaveChangesAsync();
            updated.IsUpdated = true;
            updated.Message="Employee Details Updated Successfully";
            return updated;
        }
    }
}
