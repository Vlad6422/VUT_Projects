// Author: Myron Kukhta (xkukht01)
// project: IFJ2023 imperative language translator implementation
#ifndef PARAM_LIST_H
#define PARAM_LIST_H

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include "ifj_utils.h"
#include "dynamic_string.h"

/**
 * @brief Node of parameters list.
*/
typedef struct Parametr_list_node{
    Dynamic_string_t* parametr_name;        // ds with parameters name
    Data_type_IFJ parametr_type;            // ds with parameters type
    struct Parametr_list_node* next_node;   // next node
}Parametr_list_node;


/**
 * @brief Linker list of function parameters
*/
typedef struct{
    Parametr_list_node* first;  // first node of list
    Parametr_list_node* last;   // last node of list
    int param_count;            // descr count of parametr for function
}Parametr_list_t;



// ====================== LIST OF PARAMETERS =================================


/**
 * @brief allocate memory for pointer to Parametr_list_t
 * @param param_list: target Parametr_list_t for which will be allocate memory
 * @return int: if successful 0 else 1
*/
int param_list_init(Parametr_list_t** param_list);


/**
 * @brief free memory for pointer to Parametr_list_t
 * @param param_list: target Parametr_list_t for which will be free memory
 * @return void
*/
void param_list_free(Parametr_list_t** param_list);

/**
 * @brief allocate memory for pointer to Parametr_list_node_t
 * @param param_list: target Parametr_list_node_t for which will be allocate memory
 * @return void
*/
int param_list_node_init(Parametr_list_node** new_node);



/**
 * @brief free memory for pointer to Parametr_list_node_t
 * @param param_list: target Parametr_list_node_t for which will be free memory
 * @return void
*/
void param_list_node_free(Parametr_list_node** node);



/**
 * @brief create for data new Parametr_list_node_t and insert it to list of parameters
 * @param param_list: target Parametr_list_node_t for inserting
 * @param src_name: Dynamic_string_t with name of new parametr
 * @param scr_type: type of parametr
 * @return int: if successful 0 else 1
*/
int insert_new_param_to_list(Parametr_list_t* param_list, Dynamic_string_t* src_name, Data_type_IFJ scr_type);



/**
 * @brief create for data new Parametr_list_node_t and insert it how first node of parameters list
 * @param param_list: target Parametr_list_node_t for inserting
 * @param src_name: Dynamic_string_t with name of new parametr
 * @param scr_type: type of parametr
 * @return int: if successful 0 else 1
*/
int insert_first_node_to_list(Parametr_list_t* param_list, Dynamic_string_t* src_name, Data_type_IFJ scr_type);



/**
 * @brief create for data new Parametr_list_node_t and insert it how last node of parameters list
 * @param param_list: target Parametr_list_node_t for inserting
 * @param src_name: Dynamic_string_t with name of new parametr
 * @param scr_type: type of parametr
 * @return int: if successful 0 else 1
*/
int insert_after_node_to_list(Parametr_list_t* param_list, Dynamic_string_t* src_name, Data_type_IFJ scr_type);



/**
 * @brief create for data new Parametr_list_node_t
 * @param param_list: target Parametr_list_node_t for creating
 * @param src_name: Dynamic_string_t with name of new parametr
 * @param scr_type: type of parametr
 * @return int: if successful 0 else 1
*/
int insert_data_to_parametr(Parametr_list_node* param, Dynamic_string_t* src_name, Data_type_IFJ scr_type);



/**
 * @brief check is parameters list empty
 * @param param_list: target Parametr_list_t for chekling
 * @return int: if empty 0 else 1
*/
bool is_empty(Parametr_list_t* param_list);



/**
 * @brief check if always parametr name in list
 * @param param_list
*/
bool param_name_in_param_list(Parametr_list_t* param_list, Dynamic_string_t* target_name);

#endif