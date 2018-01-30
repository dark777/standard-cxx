#include <iostream>
#include <pqxx/pqxx> 
//INSERT Operation
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

      /* Create SQL statement */
      sql = "INSERT INTO COMPANY (ID,NAME,AGE,ADDRESS,SALARY) "  \
         "VALUES (1, 'Paul', 32, 'California', 20000.00 ); " \
         "INSERT INTO COMPANY (ID,NAME,AGE,ADDRESS,SALARY) "  \
         "VALUES (2, 'Allen', 25, 'Texas', 15000.00 ); "     \
         "INSERT INTO COMPANY (ID,NAME,AGE,ADDRESS,SALARY)" \
         "VALUES (3, 'Teddy', 23, 'Norway', 20000.00 );" \
         "INSERT INTO COMPANY (ID,NAME,AGE,ADDRESS,SALARY)" \
         "VALUES (4, 'Mark', 25, 'Rich-Mond ', 65000.00 );";

      /* Create a transactional object. */
      work W(C);
      
      /* Execute SQL query */
      W.exec( sql );
      W.commit();
      std::cout << "\n\tRecords created successfully\n";
      C.disconnect ();
    }
    catch (const std::exception &e)
    {
     std::cerr << e.what() << std::endl;
     return 1;
    }
 return 0;
}