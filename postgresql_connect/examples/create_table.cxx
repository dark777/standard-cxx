#include <iostream>
#include <pqxx/pqxx> 
//Create a Table
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
       std::cout << "\n\tCan't open database\n\n";
       return 1;
      }      
      /* Create SQL statement */
      sql = "CREATE TABLE COMPANY("  \
      "ID INT PRIMARY KEY     NOT NULL," \
      "NAME           TEXT    NOT NULL," \
      "AGE            INT     NOT NULL," \
      "ADDRESS        CHAR(50)," \
      "SALARY         REAL );";

      /* Create a transactional object. */
      work W(C);
      
      /* Execute SQL query */
      W.exec( sql );
      W.commit();
      std::cout << "\n\tTable created successfully\n\n";
      C.disconnect ();
    }
    catch (const std::exception &e)
    {
     std::cerr << e.what() << std::endl;
     return 1;
    }
 return 0;
}