#include <iostream>
#include <pqxx/pqxx> 
//DELETE Operation
using namespace std;
using namespace pqxx;

int main(int argc, char* argv[])
{
 char * sql;
   
 try{
     connection C("dbname = teste user = postgres password = cohondob \
     hostaddr = 127.0.0.1 port = 5432");
     
     if(C.is_open())std::cout << "\n\tOpened database successfully: " << C.dbname() << "\n";
     else
     {
      std::cout << "\n\tCan't open database\n";
      return 1;
     }
      
     /* Create a transactional object. */
     work W(C);
     /* Create  SQL DELETE statement */
     sql = "DELETE from COMPANY where ID = 2";
     /* Execute SQL query */
     W.exec( sql );
     W.commit();
     std::cout << "\n\tRecords deleted successfully\n";
      
     /* Create SQL SELECT statement */
     sql = "SELECT * from COMPANY";
     
     /* Create a non-transactional object. */
     nontransaction N(C);
      
     /* Execute SQL query */
     result R( N.exec( sql ));
      
     /* List down all the records */
     for(result::const_iterator c = R.begin(); c != R.end(); ++c)
      {
       std::cout << "ID: " << c[0].as<int>()
                 << "Name: " << c[1].as<string>()
                 << "Age: " << c[2].as<int>()
                 << "Address: " << c[3].as<string>()
                 << "Salary: " << c[4].as<float>()
                 << "\n\n";
      }
      std::cout << "\n\tOperation done successfully\n";
      C.disconnect ();
    }
    catch(const std::exception &e)
    {
     std::cerr << e.what() << std::endl;
     return 1;
    }
 return 0;
}