#include <iostream>
#include <pqxx/pqxx> 
//SELECT Operation

int main(int argc, char* argv[])
{
 try{
      pqxx::connection login("dbname = teste user = darkstar password = darkstar hostaddr = 127.0.0.1 port = 5432");
      
      if(login.is_open())std::cout << "\n\tOpened database \""<< login.dbname() <<"\" successfully.\n\n";
      else
      {
       std::cout << "\n\tCan't open database\n\n";
       return 1;
      }
      
      // Create a non-transactional object.
      pqxx::nontransaction non_obj(login);
      
      // Create SQL statement stringstream;
      std::stringstream contsql;
      
      contsql<<"select *from person where person_id = (select count(*) from person);";
      
      // Execute SQL query 
      pqxx::result res1(non_obj.exec(contsql));
      
      for (pqxx::result::const_iterator num1 = res1.begin(); num1 != res1.end(); ++num1)
      std::cout << "\n\tNum Rows.: " << num1[0].as<int>()<<"\n";
      
      std::cout<<"\n\tQual Usuario deseja listar? ";
      std::cin>>argc;
      
      //Create SQL statement stringstream; 
      std::stringstream sql;
      sql << "select *from person where person_id=" << argc;
      
      // Create SQL statement const char*
      //const char * sql; 
      //sql = "select *from person where person_id=1";
      
      // Execute SQL query
      pqxx::result res2(non_obj.exec(sql));
      
      // List down all the records
      for (pqxx::result::const_iterator num2 = res2.begin(); num2 != res2.end(); ++num2)
      std::cout << "\n\tIdPerson.: " << num2[0].as<int>()
                << "\n\tName.....: " << num2[1].as<std::string>()
                << "\n\tAge......: " << num2[2].as<int>()
                << "\n\tStatus...: " << num2[3].as<std::string>()
                << "\n\tIdFaz....: " << num2[4].as<int>()
                << "\n\tDate.....: " << num2[5].as<std::string>()
                << "\n\n";
      
      std::cout << "\n\tOperation done successfully\n\n";
          
     login.disconnect();
    }
    catch(const std::exception &e)
    {
     std::cerr << e.what() << std::endl;
     return 1;
    }
 return 0;
}