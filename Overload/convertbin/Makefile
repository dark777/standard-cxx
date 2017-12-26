SUB=%
OBJ=*.o
EOBJ=%.o

CXX=g++7
SRC=*.c*
ESRC=%.c*

NAME=std_bin
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
