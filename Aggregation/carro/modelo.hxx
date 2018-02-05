#ifndef MODELO_HXX
#define MODELO_HXX

#include "marca.hxx"

struct Modelo
{
 Modelo(){}
  ~Modelo(){} 
  int idModelo;
  std::string descriModelo;
 Marca marca;
};

#endif