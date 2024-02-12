// Author: Myron Kukhta (xkukht01)
// project: IFJ2023 imperative language translator implementation
#ifndef PARSER_H
#define PARSER_H
#include <stdlib.h>
#include <stdio.h>
#include <string.h>
#include "ifj_utils.h"
#include "scanner.h"
#include "symtable.h"
#include "dynamic_string.h"


/**
 * @brief utils for parser with all aux data structures
*/
typedef struct{
    FILE* input_code;                       // stdin with swift input code
    Sym_table_stack_t* stack_sym_tab;       // stack of symtables
    Dynamic_string_t* output_code;          // ds for genereted code
    Symbols_table_node_t** global_sym_tab;  // global scope
    Symbol_t* actual_symbol;                // actual analyzed symbol
    Symbol_t* function;                     // actual analyzed function
    Symbol_t* var;                          // actual analyzed variable
    Symbol_t* lhs;                          // lhs = expression
    bool in_condition;                      // if condition expression for if/loop
    bool in_function;                       // in function body
    bool was_return;                        // found return in statement list
    int if_counter;                         // counter for if structure(for generator)
    int loop_counter;                       // counter for while structure(for generator)
    int param_counter;                      // counter for parametrs (for generator)
    Dynamic_string_t* param_name_aux;       // ds for storing parametrs name in LL analyse
    Dynamic_string_t* ds_aux;               // ds aux for storing last ds in LL analyse
    Token* parser_token;                    // token for lexical analyse
}Parser_utils_t;



/*================================================
                PARSER CERNAL
/================================================*/



/**
 * @brief Cernal of transletor. Support communication between LEXYC, SYNTAXE, SEMANTIC
 * @param file stdin 
 * @return int code with end our program
*/
int parser(FILE* input_code);



/**
 * @brief initialization Parser_utils_t parser_utils
 * @param parser_utils wich will be initializeted
 * @param input_code stdin
 * @return int: if successful 0 else 1
*/
int parser_utils_init(Parser_utils_t** parser_utils, FILE* input_code);



/**
 * @brief free memory of struct Parser_utils_t
 * @param parser_utils wich will be cleaned
 * @return None 
*/
void parser_utils_free(Parser_utils_t** parser_utils);



/**
 * @brief push to Global Scope swifts build-in function
 * @param parser_utils engine for parser
 * @return int: if successful 0 else > 0
*/
int push_build_in_func(Parser_utils_t* parser_utils);



/**
 * @brief check if new token have expected type
 * @param token new token
 * @param expected_type type wich expect in actual rule
 * @return  int: if expected type 0 else > 0
*/
int expect_token_type(Token* token, Data_type expected_type);



/**
 * @brief check if new token is expected keyword 
 * @param token new token
 * @param expected_key_word keyword wich expect in actual rule
 * @return int: if expected type 0 else > 0
*/
int expect_keyword(Token* token, Keyword expected_key_word);



/**
 * @brief overtyping a type(Int?,Double?,String?) that contains nil value to type(Int,Double,String) without nil value
 * @param past_data_type prevuse type
 * @return  int: if successful 0 else > 0
*/
void retyping_from_nil(Data_type_IFJ* past_data_type);



/**
 * @brief check new token is term(id||literal)
 * @param token new token
 * @return bool: if term true else false
*/
bool is_term(Token* token);



/**
 * @brief check new token is literal(int/doubl/string value)
 * @param token stored token from scanner
 * @return bool: if literal true else false
*/
bool is_literal_value(Token* token);




/**
 * @brief check if optional type(Int?/String?/Double?)
 * @param data_type input type
 * @return bool: if optional type true else false
*/
bool is_optional_type(Data_type_IFJ data_type);



/**
 * @brief check if id is initialized variable(Var/Const)
 * @param parser_utils engine for parser
 * @return bool: if variable true else false
*/
int check_is_var(Parser_utils_t* parser_utils, Dynamic_string_t* var_name);



/**
 * @brief check is can use token on the same line with other token from another synt rule
 * @param new_token token which checking
 * @return if valid then true else false
*/
bool is_valid_on_same_line(Token* new_token);



/**
 * @brief reconvert type of token const value to ifj type
 * @param token stored token from scanner
 * @return Data_type_IFJ
*/
Data_type_IFJ extract_data_type_from_token(Token* token);



/**
 * @brief reconvert type of token keyword of types value to ifj type
 * @param token stored token from scanner
 * @return Data_type_IFJ
*/
Data_type_IFJ extract_data_type_from_type_word(Keyword kw);



/**
 * @brief check if value with src_type type can be writed to memmory obj with dst_type type
 * @param src_type type of assigment value
 * @param dst_type type of target mem obj
 * @return bool: if valid true else false
*/
bool type_with_nil_compatabilty(Data_type_IFJ src_type, Data_type_IFJ dst_type);



/**
 * @brief check if arg with src_type type can be writed to param with dst_type type
 * @param src_type type of assigment value
 * @param dst_type type of target mem obj
 * @return bool: if valid true else false
*/
int args_type_compatability(Symbol_t* func, Data_type_IFJ src_type, Data_type_IFJ* dst_type);



/*================================================
                <SWIFT_PROG>
/================================================*/



/**
 * @brief LL-rule <SWIFT_PROG>
 * @param parser_utils wich will be cleaned
 * @return int: if successful 0 else > 0
*/
int swift_prog(Parser_utils_t* parser_utils);



/**
 * @brief check next rule is <FUNCTION_DEFINITION>
 * @param parser_utils engine for parser
 * @return bool: if valid true else false
*/
bool is_function(Parser_utils_t* parser_utils);



/**
 * @brief check next rule is <STATEMENT_LIST>
 * @param parser_utils engine for parser
 * @return bool: if valid true else false
*/
bool is_statement_list(Parser_utils_t* parser_utils);



/**
 * @brief LL-rule <TYPE_RULE>
 * @param parser_utils engine for parser
 * @param dst_type type of target memmory obj
 * @return int: if successful 0 else > 0
*/
int type_rule(Parser_utils_t* parser_utils, Data_type_IFJ* dst_type);



/*================================================
                <STATEMENT_LISR>
/================================================*/


/**
 * @brief LL-rule <STATEMENT_LIST>
 * @param parser_utils engine for parser
 * @return int: if successful 0 else > 0
*/
int statement_list_rule(Parser_utils_t* parser_utils);



/**
 * @brief LL-rule <IF>
 * @param parser_utils engine for parser
 * @return int: if successful 0 else > 0
*/
int if_rule(Parser_utils_t* parser_utils);



/**
 * @brief LL-rule <LET_ID>
 * @param parser_utils engine for parser
 * @return int: if successful 0 else > 0
*/
int let_rule(Parser_utils_t* parser_utils);



/**
 * @brief LL-rule <WHILE>
 * @param parser_utils engine for parser
 * @return int: if successful 0 else > 0
*/
int while_rule(Parser_utils_t* parser_utils);


/**
 * @brief LL-rule <ID_DEF>
 * @param parser_utils engine for parser
 * @param sym_class type of symbol class(function/variable/constant)
 * @return int: if successful 0 else > 0
*/
int id_def_rule(Parser_utils_t* parser_utils, Symbols_class sym_class);


/**
 * @brief LL-rule <ID_TYPE_DEFITION>
 * @param parser_utils engine for parser
 * @param dst_type pointer to actual symbol type
 * @param optional optional(true) , mandatory(false)
 * @return int: if successful 0 else > 0
*/
int id_type_definition_rule(Parser_utils_t* parser_utils, Data_type_IFJ* dst_type, bool optional);


/**
 * @brief LL-rule <ID_RULE>
 * @param parser_utils engine for parser
 * @return int: if successful 0 else > 0
*/
int id_rule(Parser_utils_t* parser_utils);


/**
 * @brief LL-rule <ASSIGMENT>
 * @param parser_utils engine for parser
 * @param optional optional(true) , mandatory(false)
 * @return int: if successful 0 else > 0
*/
int assigment_rule(Parser_utils_t* parser_utils, bool optional);



/*================================================
                <FUNCTION>
/================================================*/


/**
 * @brief LL-rule <FUNCTION_DEFINITION>
 * @param parser_utils engine for parser
 * @return int: if successful 0 else > 0
*/
int function_rule(Parser_utils_t* parser_utils);


/**
 * @brief LL-rule <PARAM_LIST>
 * @param parser_utils engine for parser
 * @return int: if successful 0 else > 0
*/
int param_list_rule(Parser_utils_t* parser_utils);


/**
 * @brief LL-rule <PARAM>
 * @param parser_utils engine for parser
 * @param param actual parametr in parametr list
 * @return int: if successful 0 else > 0
*/
int param_rule(Parser_utils_t* parser_utils, Parametr_list_node* param);


/**
 * @brief LL-rule <NEXT_PARAM>
 * @param parser_utils engine for parser
 * @param param actual parametr in parametr list
 * @return int: if successful 0 else > 0
*/
int next_param_rule(Parser_utils_t* parser_utils, Parametr_list_node* param);



/**
 * @brief LL-rule <FUNC_TYPE_DEFINTION>
 * @param parser_utils engine for parser
 * @return int: if successful 0 else > 0
*/
int func_type_definition_rule(Parser_utils_t* parser_utils);

/**
 * @brief LL-rule <RETURN>
 * @param parser_utils engine for parser
 * @return int: if successful 0 else > 0
*/
int return_rule(Parser_utils_t* parser_utils);



/**
 * @brief LL-rule <CALL_FUNCTION>
 * @param parser_utils engine for parser
 * @return int: if successful 0 else > 0
*/
int call_function_rule(Parser_utils_t* parser_utils, bool assigm_opt);


/**
 * @brief LL-rule <ARG_LIST>
 * @param parser_utils engine for parser
 * @return int: if successful 0 else > 0
*/
int argument_list_rule(Parser_utils_t* parser_utils);



/**
 * @brief LL-rule <ARG>
 * @param parser_utils engine for parser
 * @param param actual parametr in parametr list
 * @return int: if successful 0 else > 0
*/
int argument_rule(Parser_utils_t* parser_utils, Parametr_list_node* param);



/**
 * @brief LL-rule <NEXT_ARG>
 * @param parser_utils engine for parser
 * @param param actual parametr in parametr list
 * @return int: if successful 0 else > 0
*/
int next_argument_rule(Parser_utils_t* parser_utils, Parametr_list_node* param);



/**
 * @brief LL-rule <WRITE_ARG_LIST>
 * @param parser_utils engine for parser
 * @return int: if successful 0 else > 0
*/
int write_arg_list_rule(Parser_utils_t* parser_utils);



/**
 * @brief LL-rule <WRITE_ARG>
 * @param parser_utils engine for parser
 * @param param actual parametr in parametr list
 * @return int: if successful 0 else > 0
*/
int write_arg(Parser_utils_t* parser_utils);



/**
 * @brief LL-rule <WRITE_NEXT_ARG>
 * @param parser_utils engine for parser
 * @param param actual parametr in parametr list
 * @return int: if successful 0 else > 0
*/
int write_next_arg(Parser_utils_t* parser_utils);



//================================================


int check_param_count(Parser_utils_t* parser_utils);
#endif