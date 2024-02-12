// Author: Myron Kukhta (xkukht01)
// project: IFJ2023 imperative language translator implementation
#ifndef SYMTABLE_H
#define SYMTABLE_H
#include <stdlib.h>
#include <stdio.h>
#include "ifj_utils.h"
#include "param_list.h"
#include "dynamic_string.h"
#include "scanner.h"

//  identific symbol
typedef enum{
    SYMBOL_NOT_SET,
    SYMBOL_VAR,
    SYMBOL_CONST,
    SYMBOL_FUNCTION
}Symbols_class;


typedef struct{              
    Dynamic_string_t* id;
    Symbols_class sym_class;
    Data_type_IFJ type; 
    bool definition; 
    bool function_pre_definition;
    bool function_return;
    bool maybe_type_without_nil;
    bool var_init;
    Parametr_list_t* parameters;
}Symbol_t;


typedef struct Sym_table_node{
    Symbol_t symbol;
    unsigned int hight;
    struct Sym_table_node* left_st;
    struct Sym_table_node* right_st;
}Symbols_table_node_t;

typedef struct t_data{
    Symbols_table_node_t* tree;
    struct t_data* next;
}TData;

typedef struct{
    TData* top;
    unsigned int deep;
}Sym_table_stack_t;

/*---------- STACK OF SYMTABLE ----------*/
int sym_tab_stack_init(Sym_table_stack_t** sym_tab_stack);
int sym_tab_stack_push(Sym_table_stack_t* sym_tab_stack);
void sym_tab_pop(Sym_table_stack_t* sym_tab_stack);

/*---------- LEVEL OF SYMTABLEs STACK ----------*/
int tdata_init(TData** new_list);
void tdata_free(TData** list);

/*---------- NODE OF SYMTABLE TREE ----------*/
void sym_tab_clean(Symbols_table_node_t** sym_tab_node);
int sym_tab_node_init(Symbols_table_node_t** sym_tab_node);
int sym_tab_insert_new_id(Symbols_table_node_t** sym_tab, Symbol_t** actual_symbol, Dynamic_string_t* id, unsigned int symb);
int sym_tab_id_cmp(Dynamic_string_t* str_1, Dynamic_string_t* str_2);
int hight_calc(Symbols_table_node_t* sym_tab);
void update_hight_of_tree(Symbols_table_node_t* sym_tab);
int check_balance(Symbols_table_node_t* sym_tab);
Symbols_table_node_t* rotate_to_right(Symbols_table_node_t* sym_tab);
Symbols_table_node_t* rotate_to_left(Symbols_table_node_t* sym_tab);

/**
 * @brief check if function exist or not on scope
*/
//int sym_tab_check_func(Symbols_table_node_t* sym_tab_node, Dynamic_string_t* id_str);
Symbols_table_node_t* sym_tab_get_func(Symbols_table_node_t* sym_tab_node, Dynamic_string_t* id_str);
/**
 * @brief searching in function undefinited function
 * @param root root of tree of sym_tab where make searching
 * @return if found then error_code=3 else 0 
*/
int sym_tab_check_undef_func(Symbols_table_node_t* root);
/**
 * @brief search symbol on symbols table and return if found
*/
Symbol_t* search_on_act_scope(Symbols_table_node_t* sym_tab_node, Dynamic_string_t* targ_id);
/**
 * @brief search symbol on all scopes(from local to global)
*/
Symbol_t* search_on_all_scope(TData* list_of_scope, Dynamic_string_t* targ_id);
#endif