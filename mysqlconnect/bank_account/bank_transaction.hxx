#ifndef BANKTRANSACTION_HXX
#define BANKTRANSACTION_HXX

#include <mysql.h>
#include <string>
     
struct BankAccount;














struct BankTransaction
{
 BankTransaction(const string = "localhost", const std::string = "", const std::string = "", const std::string = "");
 ~BankTransaction();
 
 void createAccount(BankAccount*);
 
 void closeAccount(int);
 
 void deposit(int, double);
 
 void withdraw(int, double);
 
 BankAccount* getAccount(int);
 
 void printAllAccounts();
 
 void message(std::string);
 
 private:
   MYSQL* db_conn;
};

#endif   // BANKTRANSACTION_HXX