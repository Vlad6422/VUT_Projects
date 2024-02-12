// Author: Myron Kukhta (xkukht01)
// project: IFJ2023 imperative language translator implementation
#include "symtable.h"

int sym_tab_stack_init(Sym_table_stack_t** sym_tab_stack){
    *sym_tab_stack = (Sym_table_stack_t*)malloc(sizeof(Sym_table_stack_t));
    if(*sym_tab_stack!=NULL){
        (*sym_tab_stack)->top=NULL;
        (*sym_tab_stack)->deep=0;
        return OK;
    }
    return ERROR_INTERNAL;
}

int sym_tab_stack_push(Sym_table_stack_t* sym_tab_stack){
    int return_code;
    TData* new_list;
    if((return_code = tdata_init(&new_list)))
        return return_code;
    new_list->next = sym_tab_stack->top;
    sym_tab_stack->top = new_list;
    ++sym_tab_stack->deep;
    return return_code;
}

void sym_tab_pop(Sym_table_stack_t* sym_tab_stack){
    TData* aux;
    if(sym_tab_stack->top!=NULL){
        aux = sym_tab_stack->top;
        sym_tab_stack->top = (sym_tab_stack->top)->next;
        tdata_free(&aux);
        --sym_tab_stack->deep;
    }
}

int tdata_init(TData** new_list){
    *new_list = (TData*)malloc(sizeof(TData));
    if(*new_list != NULL){
        (*new_list)->tree=NULL;
        (*new_list)->next=NULL;
        return OK;
    }
    return ERROR_INTERNAL;
}

void tdata_free(TData** list){
    if(*list!=NULL){
        sym_tab_clean(&((*list)->tree));
        free(*list);
        *list=NULL;
    }
}

int sym_tab_node_free(Symbols_table_node_t** sym_tab_node){
    int return_code=OK;
    if((*sym_tab_node)==NULL){
        return OK;
    }
    sym_tab_node_free(&((*sym_tab_node)->left_st));
    sym_tab_node_free(&((*sym_tab_node)->right_st));
    if((*sym_tab_node)->symbol.sym_class==SYMBOL_FUNCTION && !return_code)
        if(!(*sym_tab_node)->symbol.definition)
            return_code=ERROR_UNDEF_FUNC_OR_REDEF_VAR;
    free(*sym_tab_node);
    return return_code;
}

void sym_tab_clean(Symbols_table_node_t** sym_tab_node){
    if((*sym_tab_node) != NULL){
        sym_tab_clean(&((*sym_tab_node)->left_st));
        sym_tab_clean(&((*sym_tab_node)->right_st));
        ds_free(&((*sym_tab_node)->symbol.id));
        param_list_free(&((*sym_tab_node)->symbol.parameters));
        free(*sym_tab_node);
        *sym_tab_node = NULL;
    }
}
int hight_calc(Symbols_table_node_t* sym_tab){
    if(sym_tab == NULL) return 0;
    return sym_tab->hight;
}

void update_hight_of_tree(Symbols_table_node_t* sym_tab){
    int left_hight = hight_calc(sym_tab->left_st);
    int right_hight = hight_calc(sym_tab->right_st);
    sym_tab->hight = (left_hight > right_hight ? left_hight : right_hight) + 1;
}

int check_balance(Symbols_table_node_t* sym_tab){
    if(sym_tab == NULL) return 0;
    return hight_calc(sym_tab->left_st) - hight_calc(sym_tab->right_st);
}

Symbols_table_node_t* rotate_to_right(Symbols_table_node_t* sym_tab){
    Symbols_table_node_t* aux = sym_tab->left_st;
    Symbols_table_node_t* t2 = aux->right_st;

    aux->right_st = sym_tab;
    sym_tab->left_st = t2;

    update_hight_of_tree(sym_tab);
    update_hight_of_tree(aux);

    return aux;
}

Symbols_table_node_t* rotate_to_left(Symbols_table_node_t* sym_tab){
    Symbols_table_node_t* aux = sym_tab->right_st;
    Symbols_table_node_t* t2 = aux->left_st;
    aux->left_st = sym_tab;
    sym_tab->right_st = t2;
    update_hight_of_tree(sym_tab);
    update_hight_of_tree(aux);

    return aux;
}

int sym_tab_insert_new_id(Symbols_table_node_t** sym_tab, Symbol_t** actual_symbol, Dynamic_string_t* id, unsigned int symb){
    int return_code, branch, balance;
    return_code=ERROR_INTERNAL;
    if(*sym_tab==NULL){
        if(!(return_code = sym_tab_node_init(sym_tab))){
            if(!(return_code = ds_copy(id, ((*sym_tab)->symbol).id))){
                (*sym_tab)->hight = 1;
                ((*sym_tab)->symbol).sym_class=symb;
                (*sym_tab)->symbol.definition = false;
                (*sym_tab)->symbol.function_pre_definition = false;
                (*sym_tab)->symbol.var_init = false;
                (*sym_tab)->symbol.maybe_type_without_nil = false;
                (*sym_tab)->symbol.function_return = false;
                (*sym_tab)->symbol.type = IFJ_NOT_SET_TYPE;
                (*sym_tab)->left_st=NULL;
                (*sym_tab)->right_st=NULL;
                (*actual_symbol)=&((*sym_tab)->symbol);
                return OK;
            }
        }
    }
    else{
        branch=sym_tab_id_cmp(id, ((*sym_tab)->symbol).id);
        if(branch==0){
            if((*sym_tab)->symbol.sym_class==SYMBOL_FUNCTION){
                fprintf(stderr,"Error code: %d! Function \"%s\" alwayse exist on actual scope.\n", ERROR_OTHER_SEMANTICS, id->str);
                return ERROR_OTHER_SEMANTICS;    
            }
            else{
                fprintf(stderr,"Error code: %d! Variable \"%s\" alwayse exist on actual scope.\n", ERROR_UNDEF_FUNC_OR_REDEF_VAR, id->str);
                return ERROR_UNDEF_FUNC_OR_REDEF_VAR; 
            }
        }
        else if(branch>0)  // new id > than actual node id
            return_code = sym_tab_insert_new_id(&((*sym_tab)->right_st), actual_symbol, id, symb);
        else
            return_code = sym_tab_insert_new_id(&((*sym_tab)->left_st), actual_symbol, id, symb);
    }
    
    update_hight_of_tree(*sym_tab);
    balance = check_balance(*sym_tab);
    branch = sym_tab_id_cmp(id, (*sym_tab)->symbol.id);
    // the left one is heavier
    if (balance > 1){
        if (branch < 0) {
            *sym_tab = rotate_to_right(*sym_tab);
        } 
        else{
            (*sym_tab)->left_st = rotate_to_left((*sym_tab)->left_st);
            *sym_tab = rotate_to_right(*sym_tab);
        }
    }
    // the right one is heavier
    if (balance < -1) {
        if(branch > 0){
            *sym_tab = rotate_to_left(*sym_tab);
        }
        else{
            (*sym_tab)->right_st = rotate_to_right((*sym_tab)->right_st);
            *sym_tab = rotate_to_left(*sym_tab);
        }
    }

    return return_code;

}

int sym_tab_node_init(Symbols_table_node_t** sym_tab_node){
    *sym_tab_node = (Symbols_table_node_t*)malloc(sizeof(Symbols_table_node_t));
    if(*sym_tab_node != NULL){
        if(!ds_init(&((*sym_tab_node)->symbol.id))){
            if(!param_list_init(&((*sym_tab_node)->symbol.parameters))){
                (*sym_tab_node)->right_st = NULL;
                (*sym_tab_node)->left_st = NULL;
                return OK;
            }
            ds_free(&((*sym_tab_node)->symbol.id));
        }
        free(*sym_tab_node);
    }
    return ERROR_INTERNAL;
}

// if (str1 > that str2) return > 0
int sym_tab_id_cmp(Dynamic_string_t* str_1, Dynamic_string_t* str_2){
    int return_code;
    if(str_1->length == str_2->length)
        return_code = strcmp(str_1->str, str_2->str);
    else
        return_code = str_1->length - str_2->length;
    return return_code;
}

Symbol_t* search_on_act_scope(Symbols_table_node_t* sym_tab_node, Dynamic_string_t* targ_id){
    int return_code;
    if(sym_tab_node == NULL){
        return NULL;
    }
    return_code = sym_tab_id_cmp(sym_tab_node->symbol.id, targ_id);
    if(return_code==0){
        if(!sym_tab_node->symbol.definition && (sym_tab_node->symbol.sym_class == SYMBOL_CONST || sym_tab_node->symbol.sym_class == SYMBOL_VAR)){
            return NULL;
        }
        return &(sym_tab_node->symbol);
    }
    else if(return_code > 0){  // ????
        return search_on_act_scope(sym_tab_node->left_st, targ_id);
    }
    else{
        return search_on_act_scope(sym_tab_node->right_st, targ_id);
    }
}


Symbol_t* search_on_all_scope(TData* list_of_scope, Dynamic_string_t* targ_id){
    TData* aux_scope;
    Symbol_t* target_symb;
    aux_scope = list_of_scope;
    do{
        target_symb = search_on_act_scope(aux_scope->tree, targ_id);
        if(target_symb == NULL){
            aux_scope = aux_scope->next;
            continue;
        }
        break;
    }while(aux_scope != NULL);
    return target_symb;
}

/*
int sym_tab_check_func(Symbols_table_node_t* sym_tab_node, Dynamic_string_t* id_str){
    int return_code;
    if(sym_tab_node == NULL){  // not found function
        return 1;
    }
    return_code = sym_tab_id_cmp(id_str, sym_tab_node->symbol.id);  // cmp id
    if(!return_code){  // same
        if(sym_tab_node->symbol.sym_class == SYMBOL_FUNCTION){  // is function
            return 0;  // found
        }
        return 1;
    }
    else if(return_code > 0){  // to left
        return sym_tab_check_func(sym_tab_node->right_st, id_str);
    }
    else{  // to right
        return sym_tab_check_func(sym_tab_node->left_st, id_str);
    }
}*/
Symbols_table_node_t* sym_tab_get_func(Symbols_table_node_t* sym_tab_node, Dynamic_string_t* id_str){
    int return_code;
    if(sym_tab_node == NULL){  // not found function
        return NULL;
    }
    return_code = sym_tab_id_cmp(id_str, sym_tab_node->symbol.id);  // cmp id
    if(!return_code){  // same
        if(sym_tab_node->symbol.sym_class == SYMBOL_FUNCTION){  // is function
            return sym_tab_node;  // found
        }
        return NULL;
    }
    else if(return_code > 0){  // to left
        return sym_tab_get_func(sym_tab_node->right_st, id_str);
    }
    else{  // to right
        return sym_tab_get_func(sym_tab_node->left_st, id_str);
    }
}



int sym_tab_check_undef_func(Symbols_table_node_t* root){
    int return_code=OK;
    if (root != NULL) {
        if(root->symbol.sym_class == SYMBOL_FUNCTION && !root->symbol.definition){
            return_code = ERROR_UNDEF_FUNC_OR_REDEF_VAR;
            fprintf(stderr, "Error code: %d! Function \"%s\" without definition.\n", return_code, root->symbol.id->str);
            return return_code;
        }
        if((return_code = sym_tab_check_undef_func(root->left_st))) return return_code;      // recursive calling left side of tree
        if((return_code = sym_tab_check_undef_func(root->right_st))) return return_code;     // recursive calling right side of tree
    }
    return return_code;
}