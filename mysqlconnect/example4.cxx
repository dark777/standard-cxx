#include <memory>
#include <sstream>
#include <iostream>
#include <stdexcept>

#include <mysql_connection.h>
#include <mysql_driver.h>

#include <cppconn/driver.h>
#include <cppconn/exception.h>
#include <cppconn/resultset.h>
#include <cppconn/statement.h>

/**
 * https://stackoverflow.com/questions/14570347/mysql-connector-c-bad-access-crash/14573340#14573340
 * Usage example for Driver, Connection, (simple) Statement, ResultSet
 */

std::string url("tcp://127.0.0.1:3306");
std::string user("root");
std::string pass("");
std::string database("teste");


int main(int argc, const char **argv)
{
 try{
    
     sql::Driver *driver;
     sql::Connection *con;
     sql::Statement *stmt;
     sql::ResultSet *res;
     sql::PreparedStatement *pstmt;
    
     driver = sql::mysql::get_driver_instance();
     con = driver->connect(url, user, pass);
     con->setSchema(database);
     stmt = con->createStatement();
    
     res = stmt->executeQuery("select * from cadastros");
     
     while(res->next())std::cout << res->getInt64("id_faz") << "\n"; // type may need to be different
    
     delete con;
     delete stmt;
     delete res;
    } 
    catch (sql::SQLException &e) 
    {
     std::cout << "# ERR: SQLException in " 
               << __FILE__
               << "(" << __FUNCTION__ 
               << ") on line " 
               << __LINE__ 
               << "\n# ERR: " 
               << e.what()
               << " (MySQL error code: " 
               << e.getErrorCode()
               << ", SQLState: " 
               << e.getSQLState() 
               << " )\n";
    }
  return 0;  
}