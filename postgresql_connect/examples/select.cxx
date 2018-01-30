#include <iostream>
#include <pqxx/pqxx> 
//SELECT Operation
using namespace std;
using namespace pqxx;

int main(int argc, char* argv[])
{
 char * sql;
   
 try{
      connection C("dbname = testdb user = postgres password = cohondob \
      hostaddr = 127.0.0.1 port = 5432");
      if(C.is_open())std::cout << "\n\tOpened database successfully: " << C.dbname() << "\n";
      else
      {
       std::cout << "\n\tCan't open database\n";
       return 1;
      }
      
      /* Create SQL statement */
      sql = "SELECT * from COMPANY";

      /* Create a non-transactional object. */
      nontransaction N(C);
      
      /* Execute SQL query */
      result R( N.exec( sql ));
      
      /* List down all the records */
      for (result::const_iterator c = R.begin(); c != R.end(); ++c)
      {
       std::cout << "\n\tID: " << c[0].as<int>()
                 << "\n\tName: " << c[1].as<std::string>()
                 << "\n\tAge: " << c[2].as<int>()
                 << "\n\tAddress: " << c[3].as<std::string>()
                 << "\n\tSalary: " << c[4].as<float>()
                 << "\n\n";
      }
     std::cout << "\n\tOperation done successfully\n";
     C.disconnect ();
    }
    catch (const std::exception &e)
    {
     std::cerr << e.what() << std::endl;
     return 1;
    }

   return 0;
} 
