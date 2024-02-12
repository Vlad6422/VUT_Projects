// Author: Myron Kukhta (xkukht01)
// project: IFJ2023 imperative language translator implementation
#include <stdio.h>
#include <stdlib.h>
#include "ifj_utils.h"
#include "parser.h"
int main(){
    int ifj_return_code;
    FILE* input_code;
    input_code = stdin;
    ifj_return_code = parser(input_code);
    fprintf(stderr,"return_code: %d\n", ifj_return_code);
    return ifj_return_code;
}