#include <iostream>

struct Motor
{
 Motor(){}
 ~Motor()
  {
   idMotor=0; 
   if(descriMotor.length() != 0)descriMotor.clear();
   if(descriPotencia.length() != 0)descriPotencia.clear();
  }
  int idMotor;
  std::string descriMotor;
 std::string descriPotencia;
};
