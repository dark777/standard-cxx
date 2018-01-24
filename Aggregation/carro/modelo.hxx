#include "marca.hxx"

struct Modelo
{
 Modelo(){}
 ~Modelo()
  {
   idModelo=0;
   if(descriModelo.length() != 0)descriModelo.clear();
  } 
  int idModelo;
  std::string descriModelo;
 Marca marca;
};
