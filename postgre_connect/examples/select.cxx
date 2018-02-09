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
      
      std::cout<<"\n\tQual Usuario deseja listar? ";
      std::cin>>argc;
      
      //Create SQL statement stringstream; 
      std::stringstream sql;
      sql << "select *from person where person_id=" << argc;
      
      
      // Create SQL statement const char*
      //const char * sql; 
      //sql = "select *from person where person_id=1";

      /* Create a non-transactional object. */
      pqxx::nontransaction non_obj(login);
      
      /* Execute SQL query */
      pqxx::result res(non_obj.exec(sql));
      
      /* List down all the records */
      for (pqxx::result::const_iterator c = res.begin(); c != res.end(); ++c)
       std::cout << "\n\tIdPerson.: " << c[0].as<int>()
                 << "\n\tName.....: " << c[1].as<std::string>()
                 << "\n\tAge......: " << c[2].as<int>()
                 << "\n\tStatus...: " << c[3].as<std::string>()
                 << "\n\tIdFaz....: " << c[4].as<int>()
                 << "\n\tDate.....: " << c[5].as<std::string>()
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