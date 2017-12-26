SUB=%
OBJ=*.o
EOBJ=%.o

CXX=g++
SRC=*.c*
ESRC=%.c*

NAME=name
EBIN=%$(NAME)
BIN=$(NAME)

all: $(NAME)

$(EOBJ):$(ESRC)      

	$(CXX) -c $(SRC) 

$(EBIN):$(EOBJ)      

	$(CXX) -o $(BIN) $(OBJ) 

$(SUB):$(EBIN)               

	./$(BIN)   
	rm -rf $*
