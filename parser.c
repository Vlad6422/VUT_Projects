// Author: Myron Kukhta (xkukht01)
// project: IFJ2023 imperative language translator implementation
#include "parser.h"
#include "expression.h"

//#include "generator.h"

/*================================================
                PARSER CERNAL
/================================================*/



int parser(FILE* input_code){
    int return_code;
    Parser_utils_t* parser_utils;
    if(!(return_code = parser_utils_init(&parser_utils, input_code))){
        if((return_code = push_build_in_func(parser_utils))) return return_code;  // push swifts embedded function
        if(!(return_code = get_token(parser_utils->parser_token, parser_utils->input_code))){
            parser_utils->parser_token->isNLINE = true;  // first reading
            return_code = swift_prog(parser_utils);  // <SWIFT_PROG>
            if(!return_code){return_code = sym_tab_check_undef_func(*(parser_utils->global_sym_tab));}  // search undefinited function
            do{
                sym_tab_pop(parser_utils->stack_sym_tab);
            }while(parser_utils->stack_sym_tab->top!=NULL);  // clean stack
        }
    }
    parser_utils_free(&parser_utils);
    return return_code;
}



int parser_utils_init(Parser_utils_t** parser_utils, FILE* input_code){
    int return_code;
    *parser_utils = (Parser_utils_t*)malloc(sizeof(Parser_utils_t));
    if(*parser_utils == NULL) return ERROR_INTERNAL;
    if((return_code = token_init(&((*parser_utils)->parser_token)))) return return_code;
    if((return_code = sym_tab_stack_init(&((*parser_utils)->stack_sym_tab)))) return return_code;
    if((return_code = sym_tab_stack_push((*parser_utils)->stack_sym_tab))) return return_code;
    (*parser_utils)->global_sym_tab = &(((*parser_utils)->stack_sym_tab)->top->tree);
    (*parser_utils)->input_code=input_code;
    (*parser_utils)->if_counter=0;
    (*parser_utils)->loop_counter=0;
    (*parser_utils)->in_condition = false;
    (*parser_utils)->in_function = false;
    (*parser_utils)->was_return = false;
    ds_init(&((*parser_utils)->param_name_aux));
    ds_init(&((*parser_utils)->ds_aux));
    ds_init(&((*parser_utils)->output_code));
    return OK;

}



void parser_utils_free(Parser_utils_t** parser_utils){
    if((*parser_utils)!=NULL){
        token_free(&((*parser_utils)->parser_token));
        if((*parser_utils)->stack_sym_tab!=NULL){
            free((*parser_utils)->stack_sym_tab);
        }
        ds_free(&((*parser_utils)->param_name_aux));
        ds_free(&((*parser_utils)->ds_aux));
        ds_free(&((*parser_utils)->output_code));
        free(*parser_utils);
    }
}




int push_build_in_func(Parser_utils_t* parser_utils){
    int return_code;
    
    // func write ( term1 , term2 , ..., term𝑛 )
    ds_free(&(parser_utils->ds_aux));
    ds_init(&(parser_utils->ds_aux));
    ds_add_chars(parser_utils->ds_aux, FOO_WRITE);
    if((return_code = sym_tab_insert_new_id(&(parser_utils->stack_sym_tab->top->tree),  &(parser_utils->function), parser_utils->ds_aux, SYMBOL_FUNCTION))) return return_code;
    parser_utils->function->type = IFJ_VOID;
    parser_utils->function->definition = true;
    
    // func readString() -> String?
    ds_free(&(parser_utils->ds_aux));
    ds_init(&(parser_utils->ds_aux));
    ds_add_chars(parser_utils->ds_aux, FOO_READ_STRING);
    if((return_code = sym_tab_insert_new_id(&(parser_utils->stack_sym_tab->top->tree),  &(parser_utils->function), parser_utils->ds_aux, SYMBOL_FUNCTION))) return return_code;
    parser_utils->function->parameters->param_count = 0;
    parser_utils->function->type = IFJ_STR_NIL;
    parser_utils->function->definition = true;
    
    // func readInt() -> Int?
    ds_free(&(parser_utils->ds_aux));
    ds_init(&(parser_utils->ds_aux));
    ds_add_chars(parser_utils->ds_aux, FOO_READ_INT);
    if((return_code = sym_tab_insert_new_id(&(parser_utils->stack_sym_tab->top->tree),  &(parser_utils->function), parser_utils->ds_aux, SYMBOL_FUNCTION)))  return return_code;
    parser_utils->function->parameters->param_count = 0;
    parser_utils->function->type = IFJ_INT_NIL;
    parser_utils->function->definition = true;
    

    // func readDouble() -> Double?
    ds_free(&(parser_utils->ds_aux));
    ds_init(&(parser_utils->ds_aux));
    ds_add_chars(parser_utils->ds_aux, FOO_READ_DOUBLE);
    if((return_code = sym_tab_insert_new_id(&(parser_utils->stack_sym_tab->top->tree),  &(parser_utils->function), parser_utils->ds_aux, SYMBOL_FUNCTION))) return return_code;
    parser_utils->function->parameters->param_count = 0;
    parser_utils->function->type = IFJ_DOUB_NIL;
    parser_utils->function->definition = true;
    
    // func Int2Double(_ term ∶ Int) -> Double
    ds_free(&(parser_utils->ds_aux));
    ds_init(&(parser_utils->ds_aux));
    ds_add_chars(parser_utils->ds_aux, FOO_INT_TO_DOUBLE);
    if((return_code = sym_tab_insert_new_id(&(parser_utils->stack_sym_tab->top->tree),  &(parser_utils->function), parser_utils->ds_aux, SYMBOL_FUNCTION))) return return_code;
    ds_free(&(parser_utils->ds_aux));
    ds_init(&(parser_utils->ds_aux));
    ds_add_chars(parser_utils->ds_aux, UNDO);
    if((return_code = insert_new_param_to_list(parser_utils->function->parameters, parser_utils->ds_aux, IFJ_INT))) return return_code;
    parser_utils->function->parameters->param_count = 1;
    parser_utils->function->type = IFJ_DOUB;
    parser_utils->function->definition = true;
    
    // func Double2Int(_ term ∶ Double) -> Int
    ds_free(&(parser_utils->ds_aux));
    ds_init(&(parser_utils->ds_aux));
    ds_add_chars(parser_utils->ds_aux, FOO_DOUBLE_TO_INT);
    if((return_code = sym_tab_insert_new_id(&(parser_utils->stack_sym_tab->top->tree),  &(parser_utils->function), parser_utils->ds_aux, SYMBOL_FUNCTION))) return return_code;
    ds_free(&(parser_utils->ds_aux));
    ds_init(&(parser_utils->ds_aux));
    ds_add_chars(parser_utils->ds_aux, UNDO);
    if((return_code = insert_new_param_to_list(parser_utils->function->parameters, parser_utils->ds_aux, IFJ_DOUB))) return return_code;
    parser_utils->function->parameters->param_count = 1;
    parser_utils->function->type = IFJ_INT;
    parser_utils->function->definition = true;
    
    // func length(_ 𝑠 : String) -> Int
    ds_free(&(parser_utils->ds_aux));
    ds_init(&(parser_utils->ds_aux));
    ds_add_chars(parser_utils->ds_aux, FOO_LENGTH);
    if((return_code = sym_tab_insert_new_id(&(parser_utils->stack_sym_tab->top->tree),  &(parser_utils->function), parser_utils->ds_aux, SYMBOL_FUNCTION))) return return_code;
    ds_free(&(parser_utils->ds_aux));
    ds_init(&(parser_utils->ds_aux));
    ds_add_chars(parser_utils->ds_aux, UNDO);
    if((return_code = insert_new_param_to_list(parser_utils->function->parameters, parser_utils->ds_aux, IFJ_STR))) return return_code;
    parser_utils->function->parameters->param_count = 1;
    parser_utils->function->type = IFJ_INT;
    parser_utils->function->definition = true;
    
    //func ord(_ 𝑐 : String) -> Int
    ds_free(&(parser_utils->ds_aux));
    ds_init(&(parser_utils->ds_aux));
    ds_add_chars(parser_utils->ds_aux, FOO_ORD);
    if((return_code = sym_tab_insert_new_id(&(parser_utils->stack_sym_tab->top->tree),  &(parser_utils->function), parser_utils->ds_aux, SYMBOL_FUNCTION))) return return_code;
    ds_free(&(parser_utils->ds_aux));
    ds_init(&(parser_utils->ds_aux));
    ds_add_chars(parser_utils->ds_aux, UNDO);
    if((return_code = insert_new_param_to_list(parser_utils->function->parameters, parser_utils->ds_aux, IFJ_STR))) return return_code;
    parser_utils->function->parameters->param_count = 1;
    parser_utils->function->type = IFJ_INT;
    parser_utils->function->definition = true;
    
    //func chr(_ 𝑖 : Int) -> String
    ds_free(&(parser_utils->ds_aux));
    ds_init(&(parser_utils->ds_aux));
    ds_add_chars(parser_utils->ds_aux, FOO_CHR);
    if((return_code = sym_tab_insert_new_id(&(parser_utils->stack_sym_tab->top->tree),  &(parser_utils->function), parser_utils->ds_aux, SYMBOL_FUNCTION))) return return_code;
    ds_free(&(parser_utils->ds_aux));
    ds_init(&(parser_utils->ds_aux));
    ds_add_chars(parser_utils->ds_aux, UNDO);
    if((return_code = insert_new_param_to_list(parser_utils->function->parameters, parser_utils->ds_aux, IFJ_INT))) return return_code;
    parser_utils->function->parameters->param_count = 1;
    parser_utils->function->type = IFJ_STR;
    parser_utils->function->definition = true;
    
    //func substring(of 𝑠 : String, startingAt 𝑖 : Int, endingBefore 𝑗 : Int) -> String?
    ds_free(&(parser_utils->ds_aux));
    ds_init(&(parser_utils->ds_aux));
    ds_add_chars(parser_utils->ds_aux, FOO_SUBSTRING);
    if((return_code = sym_tab_insert_new_id(&(parser_utils->stack_sym_tab->top->tree),  &(parser_utils->function), parser_utils->ds_aux, SYMBOL_FUNCTION))) return return_code;
    ds_free(&(parser_utils->ds_aux));
    ds_init(&(parser_utils->ds_aux));
    ds_add_chars(parser_utils->ds_aux, OF);
    if((return_code = insert_new_param_to_list(parser_utils->function->parameters, parser_utils->ds_aux, IFJ_STR))) return return_code;
    ds_free(&(parser_utils->ds_aux));
    ds_init(&(parser_utils->ds_aux));
    ds_add_chars(parser_utils->ds_aux, START_AT);
    if((return_code = insert_new_param_to_list(parser_utils->function->parameters, parser_utils->ds_aux, IFJ_INT))) return return_code;
    ds_free(&(parser_utils->ds_aux));
    ds_init(&(parser_utils->ds_aux));
    ds_add_chars(parser_utils->ds_aux, END_BEFOUR);
    if((return_code = insert_new_param_to_list(parser_utils->function->parameters, parser_utils->ds_aux, IFJ_INT))) return return_code;
    parser_utils->function->parameters->param_count = 3;
    parser_utils->function->type = IFJ_STR_NIL;
    parser_utils->function->definition = true;
    
    return OK;
}



/*================================================
          ENGINE FOR SPECIFIC WORK PARSER
/================================================*/



int syntaxe_first_cmp_second(unsigned int aux1, unsigned int aux2){
    if(aux1 != aux2) return ERROR_SYNTAXE;
    return OK;
}


int expect_token_type(Token* token, Data_type expected_type){
    int return_code;
    return_code = syntaxe_first_cmp_second(expected_type, token->type);
    return return_code;
}


int expect_keyword(Token* token, Keyword expected_key_word){
    int return_code;
    if((return_code = expect_token_type(token, TOKEN_KEY))) return return_code;
    return_code = syntaxe_first_cmp_second(expected_key_word, token->attribute.keyword);
    return return_code;
}

Data_type_IFJ extract_data_type_from_token(Token* token){
    if (token->type == TOKEN_INT)
        return IFJ_INT;
    else if (token->type == TOKEN_STRING)
        return IFJ_STR;
    else if (token->type == TOKEN_DOUBLE)
        return IFJ_DOUB;
    else if (token->type == TOKEN_NIL)
        return IFJ_NIL;
}



Data_type_IFJ extract_data_type_from_type_word(Keyword kw){
    if (kw == KW_INT)
        return IFJ_INT;
    else if (kw == KW_INT_NIL)
        return IFJ_INT_NIL;
    else if (kw == KW_DOUBLE)
        return IFJ_DOUB;
    else if (kw == KW_DOUBLE_NIL)
        return IFJ_DOUB_NIL;
    else if (kw == KW_STRING)
        return IFJ_STR;
    else if (kw == KW_STRING_NIL)
        return IFJ_STR_NIL;
}

void retyping_from_nil(Data_type_IFJ* past_data_type){
    if(*past_data_type == IFJ_INT_NIL){
        *past_data_type = IFJ_INT;
    }
    else if(*past_data_type == IFJ_DOUB_NIL){
        *past_data_type = IFJ_DOUB;
    }
    else if(*past_data_type == IFJ_STR_NIL){
        *past_data_type = IFJ_STR;
    }
}



bool is_valid_on_same_line(Token* new_token){
    if(new_token->isNLINE) return true;  // legal(new line)
    else{
        if(new_token->type == TOKEN_L_CUR_BRACE || new_token->type == TOKEN_R_CUR_BRACE || new_token->type == TOKEN_EOF) return true;  // legal(on new line)
    }
    return false;  // illegal(on the same line)
}



bool is_term(Token* token){
    if(is_literal_value(token) || token->type == TOKEN_IDE) return true;
    return false;
}



bool is_literal_value(Token* token){
    if (token->type == TOKEN_INT || token->type == TOKEN_STRING || token->type == TOKEN_DOUBLE || token->type == TOKEN_NIL) return true;
    return false;
}

bool type_with_nil_compatabilty(Data_type_IFJ src_type, Data_type_IFJ dst_type){
    if((dst_type == IFJ_INT_NIL && (src_type == IFJ_INT  || src_type == IFJ_NIL)) ||
    (dst_type == IFJ_STR_NIL && (src_type == IFJ_STR ||  src_type == IFJ_NIL)) ||
    (dst_type == IFJ_DOUB_NIL && (src_type == IFJ_DOUB ||  src_type == IFJ_NIL))
    ) return true;  // intersection of the value area
    return false;
}

int args_type_compatability(Symbol_t* func, Data_type_IFJ src_type, Data_type_IFJ* dst_type){
    if(src_type != *dst_type){
        if(!type_with_nil_compatabilty(src_type, *dst_type)){
            if(!func->definition){  // pre-def
                if(*dst_type == IFJ_NOT_SET_TYPE){  // first
                    *dst_type = src_type;
                    return OK;
                }
                else if(*dst_type == IFJ_NIL){  // pre-def second 
                    if(src_type == IFJ_INT || src_type == IFJ_INT_NIL){
                        *dst_type = IFJ_INT_NIL;
                    }
                    else if(src_type == IFJ_DOUB || src_type == IFJ_DOUB_NIL){
                        *dst_type = IFJ_DOUB_NIL;
                    }
                    else if(src_type == IFJ_STR || src_type == IFJ_STR_NIL){
                        *dst_type = IFJ_STR_NIL;
                    }
                    return OK;
                }
                else if(src_type == IFJ_NIL){  // pre-def second 
                    if(*dst_type == IFJ_INT || *dst_type == IFJ_INT_NIL){
                        *dst_type = IFJ_INT_NIL;
                    }
                    else if(*dst_type == IFJ_DOUB || *dst_type == IFJ_DOUB_NIL){
                        *dst_type = IFJ_DOUB_NIL;
                    }
                    else if(*dst_type == IFJ_STR || *dst_type == IFJ_STR_NIL){
                        *dst_type = IFJ_STR_NIL;
                    }
                    return OK;
                }
                else{
                    if((src_type == IFJ_INT_NIL && *dst_type == IFJ_INT) || (src_type == IFJ_DOUB_NIL && *dst_type == IFJ_DOUB) || (src_type == IFJ_STR_NIL && *dst_type == IFJ_STR)){
                        return OK;
                    }
                }
            }
            fprintf(stderr, "Error code: %d! Unsuitable type with parametr by function \"%s\".\n", ERROR_BAD_PARAM_OR_BAD_RETURN_TYPE, func->id->str);
            return ERROR_BAD_PARAM_OR_BAD_RETURN_TYPE;
        }
    }
    return OK;
}
int func_call_type_compatability(Symbol_t* func, Symbol_t* lhs){
    if(func->type == IFJ_VOID || func->type == IFJ_NOT_SET_TYPE){  // void func or pre_def func with unknown type
        if(!func->definition){  // pre_def func with unknown type
            if(lhs->type != IFJ_NOT_SET_TYPE){
                if(!is_optional_type(lhs->type))
                    func->maybe_type_without_nil=true;
                func->type = lhs->type;
                retyping_from_nil(&(func->type));
            }
            else{
                fprintf(stderr, "Error code: %d! Cannot extract type from unknow function \"%s\".\n", ERROR_UNKNOWN_TYPE_VAR_OR_PARAM, func->id->str);
                return ERROR_UNKNOWN_TYPE_VAR_OR_PARAM;
            }
        }
        else{  // known definited func:  id = void_func()
            fprintf(stderr, "Error code: %d! Using void function \"%s\" in assigment action.\n", ERROR_TYPE_UNCOMPATABILITY_ON_EXPR, func->id->str);
            return ERROR_TYPE_UNCOMPATABILITY_ON_EXPR;
        }
    }
    else if(func->type != IFJ_NOT_SET_TYPE){  // known type for func(definited/predefinited)
        if(lhs->type == IFJ_NOT_SET_TYPE){  // set type
            lhs->type = func->type;
            return OK;
        }
        if(lhs->type != func->type){  // check type compatability
            if(!type_with_nil_compatabilty(func->type, lhs->type)){
                fprintf(stderr, "Error code: %d! Type incompatability with assigment action with function \"%s\".\n", ERROR_TYPE_UNCOMPATABILITY_ON_EXPR, func->id->str);
                return ERROR_TYPE_UNCOMPATABILITY_ON_EXPR;
            }
        }
        else{
            func->maybe_type_without_nil=true;
        }
    }
    return OK;
}

int check_is_var(Parser_utils_t* parser_utils, Dynamic_string_t* var_name){
    int return_code;
    parser_utils->var = search_on_all_scope(parser_utils->stack_sym_tab->top, var_name);
    if(parser_utils->var==NULL){
        fprintf(stderr, "Error code: %d! Undefinited variable \"%s\".", ERROR_UNDEF_OR_UNINIT_VAR, var_name->str);
        return ERROR_UNDEF_OR_UNINIT_VAR;
    }
    if(parser_utils->var->sym_class == SYMBOL_FUNCTION){
        fprintf(stderr, "Error code: %d! Undefinited variable \"%s\".", ERROR_UNDEF_OR_UNINIT_VAR, var_name->str);
        return ERROR_UNDEF_OR_UNINIT_VAR;
    }
    if(!parser_utils->var->var_init){
        fprintf(stderr, "Error code: %d! Attempt to access the memory of an uninitialized variable \"%s\".\n", ERROR_UNDEF_OR_UNINIT_VAR, parser_utils->var->id->str);
        return ERROR_UNDEF_OR_UNINIT_VAR;
    }
    return OK;
}



/*================================================
                <SWIFT_PROG>
/================================================*/



int swift_prog(Parser_utils_t* parser_utils){
    int return_code;
    
    if(!is_valid_on_same_line(parser_utils->parser_token)){  // new instruction on new line
            fprintf(stderr, "Error code: %d! Unknown syntaxe structure on line.\n", ERROR_SYNTAXE);
            return ERROR_SYNTAXE;
    }
    if(parser_utils->parser_token->type==TOKEN_EOF){  // end of analyse
        return OK;
    }
    if(is_function(parser_utils)){  // choose function rule
        return_code=ERROR_SYNTAXE;
        if(parser_utils->stack_sym_tab->deep > 1){
            fprintf(stderr,"Error code: %d! Trying definition function on local scope with depping: %d.\n", return_code, parser_utils->stack_sym_tab->deep);
            return return_code;
        }
        else{
            return_code = function_rule(parser_utils);
        }
    }
    else if(is_statement_list(parser_utils))  // choose statement rule
        return_code = statement_list_rule(parser_utils);
    else{  // unsupported syntaxe structure
        return_code = ERROR_SYNTAXE;
        fprintf(stderr, "Error code: %d! Unknown syntaxe structure on line.\n", ERROR_SYNTAXE);
    }

    if(return_code) return return_code;
    return swift_prog(parser_utils); 
}



bool is_statement_list(Parser_utils_t* parser_utils){
    if(parser_utils->parser_token->type==TOKEN_IDE)
        return true;
    else if(parser_utils->parser_token->type == TOKEN_KEY)
        if(parser_utils->parser_token->attribute.keyword == KW_IF || 
            parser_utils->parser_token->attribute.keyword == KW_VAR ||
            parser_utils->parser_token->attribute.keyword == KW_LET ||
            parser_utils->parser_token->attribute.keyword == KW_WHILE)
            return true;
    return false;
    
}



bool is_function(Parser_utils_t* parser_utils){
    if (parser_utils->parser_token->type == TOKEN_KEY)
        if(parser_utils->parser_token->attribute.keyword==KW_FUNCTION)
            return true;
    return false;
}



bool is_optional_type(Data_type_IFJ data_type){
    if(data_type == IFJ_INT_NIL || data_type == IFJ_DOUB_NIL || data_type == IFJ_STR_NIL) return true;
    return false;
}



/*================================================
                GENERAL RULES
/================================================*/



int type_rule(Parser_utils_t* parser_utils, Data_type_IFJ* dst_type){
    int return_code;
    
    Keyword types[6]={KW_INT, KW_STRING, KW_DOUBLE, KW_INT_NIL, KW_STRING_NIL, KW_DOUBLE_NIL};  // all valids types
    if(!(return_code = expect_token_type(parser_utils->parser_token, TOKEN_KEY))){
        for (int i = 0; i < 6; i++){
            if(parser_utils->parser_token->attribute.keyword==types[i]){  // it is legal type name
                *dst_type = extract_data_type_from_type_word(parser_utils->parser_token->attribute.keyword);

                return OK;
            }
        }
        return_code =  ERROR_SYNTAXE;
    }
    
    fprintf(stderr,"Error code: %d! There is no mandatory type name for the specification.\n", return_code);
    return return_code;
}



int id_type_definition_rule(Parser_utils_t* parser_utils,Data_type_IFJ* dst_type, bool optional){
    // : <TYPE>
    int return_code;

    if(!expect_token_type(parser_utils->parser_token, TOKEN_COL)){  // : 
        
        if((return_code=get_token(parser_utils->parser_token, parser_utils->input_code))) return return_code;
        if((return_code = type_rule(parser_utils, dst_type))) return return_code; // <TYPE>
        return get_token(parser_utils->parser_token, parser_utils->input_code);
    }
    if (!optional){  // if not optional type specification
        fprintf(stderr, "Error code: %d! There is no mandatory type specification.\n", ERROR_SYNTAXE);
        return ERROR_SYNTAXE;
    }
    return OK;  // e
}



/*================================================
                <STATEMENT_LIST>
/================================================*/



int statement_list_rule(Parser_utils_t* parser_utils){
    int return_code;
    
    if(!is_valid_on_same_line(parser_utils->parser_token)){
            fprintf(stderr, "Error code: %d! Unknown syntaxe structure on line.\n", ERROR_SYNTAXE);
            return ERROR_SYNTAXE;
    }
    // rules
    if(parser_utils->parser_token->type == TOKEN_KEY){
        if(parser_utils->parser_token->attribute.keyword == KW_IF && parser_utils->parser_token) return_code = if_rule(parser_utils);  // IF 
        else if(parser_utils->parser_token->attribute.keyword == KW_WHILE) return_code = while_rule(parser_utils);  // WHILE
        else if(parser_utils->parser_token->attribute.keyword == KW_VAR) return_code = id_def_rule(parser_utils, SYMBOL_VAR);  // VAR
        else if(parser_utils->parser_token->attribute.keyword == KW_LET) return_code = id_def_rule(parser_utils, SYMBOL_CONST);  // LET
        else if(parser_utils->parser_token->attribute.keyword == KW_RETURN) return_code = return_rule(parser_utils);  // return 
        else {return 0;}        // e
    }
    else if(parser_utils->parser_token->type == TOKEN_IDE)return_code = id_rule(parser_utils);  // ID
    else{return 0;}  // e
    if(return_code) return return_code;  // bad 
    return statement_list_rule(parser_utils);  // <STATEMENT_LIST>
}



int id_def_rule(Parser_utils_t* parser_utils, Symbols_class sym_class){
    int return_code;

    if((return_code=get_token(parser_utils->parser_token, parser_utils->input_code))) return return_code;   // ID
    if((return_code = expect_token_type(parser_utils->parser_token, TOKEN_IDE))){
        fprintf(stderr, "Error code:%d! Unknown syntaxe structure, expect variable id.\n", ERROR_SYNTAXE);
        return return_code;
    }
    if((return_code = sym_tab_insert_new_id(&parser_utils->stack_sym_tab->top->tree, &parser_utils->lhs, parser_utils->parser_token->attribute.string, sym_class))) return return_code;
    
    if((return_code=get_token(parser_utils->parser_token, parser_utils->input_code))) return return_code;
    if((return_code = id_type_definition_rule(parser_utils, &(parser_utils->lhs->type),true))) return return_code;  // <TYPE_DEFITION>
    if((return_code = assigment_rule(parser_utils, true))) return return_code;  // <ASSIGMENT>
    if(!parser_utils->lhs->var_init){  // lost init expression
        if(parser_utils->lhs->type == IFJ_NOT_SET_TYPE){  // lost type specification
            fprintf(stderr, "Error code: %d! There is no type specification and initialization expression for a variable \"%s\".", ERROR_SYNTAXE, parser_utils->lhs->id->str);
            return ERROR_SYNTAXE;
        }
        if(is_optional_type(parser_utils->lhs->type)){  // type with nil
            parser_utils->lhs->var_init = true;
        }
        else{  // type without nill
            parser_utils->lhs->var_init = false;    
        }
    }
    parser_utils->lhs->definition = true;
    parser_utils->var =parser_utils->lhs; 
    //gen_declareVar(parser_utils);
    return return_code;
}



int id_rule(Parser_utils_t* parser_utils){
    int return_code;
    bool flag_function = false;
    ds_copy(parser_utils->parser_token->attribute.string, parser_utils->ds_aux);
    
    if((return_code=get_token(parser_utils->parser_token, parser_utils->input_code))) return return_code;  // = or (

    if(!expect_token_type(parser_utils->parser_token, TOKEN_LPAREN)){  // function
        flag_function = true;
        if((return_code = call_function_rule(parser_utils, false))) return return_code;  // id(...)
    }
    else{ // variable 
        parser_utils->lhs = search_on_all_scope(parser_utils->stack_sym_tab->top, parser_utils->ds_aux);
        if(parser_utils->lhs == NULL){  // unfined id
                fprintf(stderr, "Error code: %d! Undeclared variable with id \"%s\".\n", ERROR_UNDEF_OR_UNINIT_VAR, parser_utils->ds_aux->str);
                return ERROR_UNDEF_OR_UNINIT_VAR;
        }
        if((return_code = assigment_rule(parser_utils, false))) return return_code;  // <ASSIGMENT_RULE>
    }
    if(!flag_function){  // if var
        if(!parser_utils->lhs->var_init)  // init var after successful expression
            parser_utils->lhs->var_init = true;
    }
    return OK;
}



int assigment_rule(Parser_utils_t* parser_utils, bool in_definition){
    
    int return_code=OK;
    Data_type_IFJ return_type;
    
    if(expect_token_type(parser_utils->parser_token, TOKEN_ASS)){  // =
        if (!in_definition){  // not options assigment 
            fprintf(stderr, "Error code: %d! Except \"=\" after variable with name \"%s\"", ERROR_SYNTAXE, parser_utils->lhs->id->str);
            return ERROR_INTERNAL;
        }
        return OK;
    }

    if((parser_utils->lhs->sym_class == SYMBOL_CONST) && parser_utils->lhs->var_init){  // trying rewrite const is bad
            fprintf(stderr, "Error code: %d! Attempt to change the value of a constant with id \"%s\".\n", ERROR_OTHER_SEMANTICS, parser_utils->lhs->id->str);
            return ERROR_OTHER_SEMANTICS;
    }
    

    if((return_code=get_token(parser_utils->parser_token, parser_utils->input_code))) return return_code;  // = id
    if(!expect_token_type(parser_utils->parser_token, TOKEN_IDE)){
        ds_copy(parser_utils->parser_token->attribute.string, parser_utils->ds_aux);  // store id from ride side by =
        parser_utils->var = search_on_all_scope(parser_utils->stack_sym_tab->top, parser_utils->ds_aux);
        if(parser_utils->var == NULL){
            if((return_code=get_token(parser_utils->parser_token, parser_utils->input_code))) return return_code;
            if(!expect_token_type(parser_utils->parser_token, TOKEN_LPAREN)){  // id(... will be validate like function
                if((return_code = call_function_rule(parser_utils, true))) return return_code;    
            }
            else{  // id... will be validate like var
                fprintf(stderr, "Error code: %d! Undeclared variable with id \"%s\".\n", ERROR_UNDEF_OR_UNINIT_VAR, parser_utils->ds_aux->str);
                return ERROR_UNDEF_OR_UNINIT_VAR;
            }        
            
        }
        else{
            if(parser_utils->var->sym_class == SYMBOL_FUNCTION){  // a = func...
                if((return_code=get_token(parser_utils->parser_token, parser_utils->input_code))) return return_code;
                if(expect_token_type(parser_utils->parser_token, TOKEN_LPAREN)){  // will be validate like undeclareted variable
                    fprintf(stderr, "Error code: %d! Undeclareted variable \"%s\".", ERROR_UNDEF_OR_UNINIT_VAR, parser_utils->var->id->str);
                    return ERROR_UNDEF_OR_UNINIT_VAR;
                }
                if((return_code = call_function_rule(parser_utils, true))) return return_code;
            }
            else{  // id = expression
                if((return_code = rule_expression(parser_utils, parser_utils->input_code))) return return_code;
            }
        }

    }
    else  // id = expression
        if((return_code = rule_expression(parser_utils, parser_utils->input_code))) return return_code;
    if(in_definition) parser_utils->lhs->var_init = true;  // was valid assigments
    return return_code;
}


int if_rule(Parser_utils_t* parser_utils){
    
    int return_code;
    int actual_index;
    bool flag_retyped = false;
    
    
    actual_index = parser_utils->if_counter;
    parser_utils->if_counter += 1;

    // <IF_EXPRESSION>
    if((return_code=get_token(parser_utils->parser_token, parser_utils->input_code))) return return_code;
    if(!(return_code = expect_keyword(parser_utils->parser_token, KW_LET))){
        if((return_code=get_token(parser_utils->parser_token, parser_utils->input_code))) return return_code;
        if((return_code=let_rule(parser_utils))) return return_code;
        flag_retyped=true;
    }
    else{
        parser_utils->in_condition=true;
        if((return_code = rule_expression(parser_utils, parser_utils->input_code))) return return_code;
        parser_utils->in_condition=false;
    }
    
    if((return_code = expect_token_type(parser_utils->parser_token, TOKEN_L_CUR_BRACE))){// { 
        fprintf(stderr, "Error code:%d! Except \"{\" for open new scope.\n", return_code);
        return return_code;
    }

    if((return_code=get_token(parser_utils->parser_token, parser_utils->input_code))) return return_code;
    parser_utils->parser_token->isNLINE=true;

    if(flag_retyped){
        if((return_code=sym_tab_stack_push(parser_utils->stack_sym_tab))) return return_code;  // creating new scope
        if((return_code = sym_tab_insert_new_id(&parser_utils->stack_sym_tab->top->tree, &parser_utils->actual_symbol, parser_utils->var->id, SYMBOL_CONST))) return return_code;
        parser_utils->actual_symbol->definition = true;
        parser_utils->actual_symbol->type = parser_utils->var->type;
        parser_utils->actual_symbol->var_init=true;
        retyping_from_nil(&(parser_utils->actual_symbol->type));  // new type without nil
    }    
    if((return_code=sym_tab_stack_push(parser_utils->stack_sym_tab))) return return_code;  // creating new scope
    if((return_code=statement_list_rule(parser_utils))) return return_code;// <STATEMENT_LIST>
    
    if(flag_retyped){
        flag_retyped=false;
        sym_tab_pop(parser_utils->stack_sym_tab);
    }
    sym_tab_pop(parser_utils->stack_sym_tab);

    if((return_code = expect_token_type(parser_utils->parser_token, TOKEN_R_CUR_BRACE))){// } 
        fprintf(stderr, "Error code:%d! Unexpected syntactic structure.\n", return_code);
        return return_code;
    };

    // delete new scope
    return_code = get_token(parser_utils->parser_token, parser_utils->input_code);
    parser_utils->parser_token->isNLINE=true;



    if((return_code = expect_keyword(parser_utils->parser_token, KW_ELSE))){    // ELSE
        fprintf(stderr, "Error code: %d! Expect keyword \"else\" after if-block.\n", return_code);
        return return_code;
    }

    if((return_code=get_token(parser_utils->parser_token, parser_utils->input_code))) return return_code;
    if((return_code = expect_token_type(parser_utils->parser_token, TOKEN_L_CUR_BRACE))){// {
        fprintf(stderr, "Error code:%d! Unexpected syntactic structure. Except\"{\" after \"else\" keyword.\n", return_code);
        return return_code;
    };

    if((return_code=sym_tab_stack_push(parser_utils->stack_sym_tab))) return return_code;  // creating new scope
    if((return_code=get_token(parser_utils->parser_token, parser_utils->input_code))) return return_code;
    parser_utils->parser_token->isNLINE=true;
    if((return_code=statement_list_rule(parser_utils))) return return_code;// <STATEMENT_LIST>
    sym_tab_pop(parser_utils->stack_sym_tab);
    
    if((return_code = expect_token_type(parser_utils->parser_token, TOKEN_R_CUR_BRACE))){// } 
        fprintf(stderr, "Error code:%d! Unexpected syntactic structure.\n", return_code);
        return return_code;
    };
    
    return get_token(parser_utils->parser_token, parser_utils->input_code);
}

int let_rule(Parser_utils_t* parser_utils){
    int return_code;

    if((return_code = expect_token_type(parser_utils->parser_token, TOKEN_IDE))){  // id
        fprintf(stderr, "Error code:%d! Unknown syntaxe structure, expect constant id.\n", return_code);
        return return_code;
    }
    if((return_code = check_is_var(parser_utils, parser_utils->parser_token->attribute.string))) return return_code;  // it should be definited variable
    return get_token(parser_utils->parser_token, parser_utils->input_code);
}


int while_rule(Parser_utils_t* parser_utils){
    int return_code;
    int actual_index;
    
    actual_index = parser_utils->loop_counter;
    parser_utils->loop_counter += 1;

    if((return_code=get_token(parser_utils->parser_token, parser_utils->input_code))) return return_code;   //WHILE <EXPRESSION>
    parser_utils->in_condition=true;
    if((return_code = rule_expression(parser_utils, parser_utils->input_code))) return return_code;
    parser_utils->in_condition=false;
    
    if((return_code = expect_token_type(parser_utils->parser_token, TOKEN_L_CUR_BRACE))){
        fprintf(stderr, "Error code:%d! Unexpected syntactic structure in while loop.\n", return_code);
        return return_code;
    };

    if((return_code=get_token(parser_utils->parser_token, parser_utils->input_code))) return return_code;
    if((return_code=sym_tab_stack_push(parser_utils->stack_sym_tab))) return return_code;  // creating new scope
    if((return_code=statement_list_rule(parser_utils))) return return_code;  // <STATEMENT_LIST>
    sym_tab_pop(parser_utils->stack_sym_tab);

    if((return_code = expect_token_type(parser_utils->parser_token, TOKEN_R_CUR_BRACE))){   // } 
        fprintf(stderr, "Error code:%d! Unexpected syntactic structure.\n", return_code);
        return return_code;
    };

    return get_token(parser_utils->parser_token, parser_utils->input_code);
}



/*================================================
                <FUNCTION>
/================================================*/



int function_rule(Parser_utils_t* parser_utils){
    int return_code;
    
    parser_utils->if_counter=0;
    parser_utils->loop_counter=0;
    parser_utils->in_function=true;
    if((return_code = get_token(parser_utils->parser_token, parser_utils->input_code))) return return_code;  // ID
    if((return_code = expect_token_type(parser_utils->parser_token, TOKEN_IDE))){
        fprintf(stderr, "Error code: %d! Except id of new function after keyword \"func\".\n", ERROR_SYNTAXE);
        return ERROR_SYNTAXE;
    }
    
    parser_utils->function = search_on_act_scope((*(parser_utils->global_sym_tab)),parser_utils->parser_token->attribute.string);  // check is function always readed
    
    if(parser_utils->function != NULL){  // id always exist on global scope
        if(parser_utils->function->sym_class != SYMBOL_FUNCTION){  // it is var
            fprintf(stderr, "Error code: %d! Trying redefinition variable with id \"%s\" like function.\n", ERROR_UNDEF_FUNC_OR_REDEF_VAR, parser_utils->function->id->str);
            return ERROR_UNDEF_FUNC_OR_REDEF_VAR;
        }
        if(parser_utils->function->definition){  // pre-definited function
            fprintf(stderr, "Error code: %d! Trying redefinition function with id \"%s\".\n", ERROR_OTHER_SEMANTICS, parser_utils->function->id->str);
            return ERROR_OTHER_SEMANTICS;
        }
    }
    else{  // normal func def
        if((return_code = sym_tab_insert_new_id(parser_utils->global_sym_tab, &parser_utils->actual_symbol, parser_utils->parser_token->attribute.string, SYMBOL_FUNCTION))) return return_code;
        parser_utils->function = parser_utils->actual_symbol;
        parser_utils->function->definition=true;
    }
    parser_utils->function->function_pre_definition=false; 
    
    if((return_code=get_token(parser_utils->parser_token, parser_utils->input_code))) return return_code; // (
    if(expect_token_type(parser_utils->parser_token, TOKEN_LPAREN)){
        fprintf(stderr, "Error code:%d! Expect \"(\" after functions id on functions defintion.\n", ERROR_SYNTAXE);
        return ERROR_SYNTAXE;
    }
    if((return_code=get_token(parser_utils->parser_token, parser_utils->input_code))) return return_code;
    parser_utils->param_counter = 0;
    if((return_code=sym_tab_stack_push(parser_utils->stack_sym_tab))) return return_code;  // creating new scope for pushing param id like const
    if((return_code = param_list_rule(parser_utils))) return return_code;  // <PARAM_LIST>    
    if((return_code = expect_token_type(parser_utils->parser_token, TOKEN_RPAREN))){ // )
        fprintf(stderr, "Error code:%d! Unexpected syntactic structure on parametrs list for function \"%s\".\n", ERROR_SYNTAXE, parser_utils->function->id->str);
        return ERROR_SYNTAXE;
    }

    if((return_code = check_param_count(parser_utils))) return return_code;

    if((return_code=get_token(parser_utils->parser_token, parser_utils->input_code))) return return_code;
    if((return_code = func_type_definition_rule(parser_utils))) return return_code;  // <FUNC_TYPE_DEFINTION>
    parser_utils->function->definition=true;
    if((return_code = expect_token_type(parser_utils->parser_token, TOKEN_L_CUR_BRACE))){// { 
        fprintf(stderr, "Error code:%d! Except \"{\" for open new \"%s\" functions scope.\n", ERROR_SYNTAXE, parser_utils->function->id->str);
        return ERROR_SYNTAXE;
    }

    if((return_code=get_token(parser_utils->parser_token, parser_utils->input_code))) return return_code;
    parser_utils->parser_token->isNLINE=true;

    if((return_code=sym_tab_stack_push(parser_utils->stack_sym_tab))) return return_code;  // creating sub scope for func body (with retyping parametrs)
    
    if((return_code=statement_list_rule(parser_utils))) return return_code;// <STATEMENT_LIST>
    
    sym_tab_pop(parser_utils->stack_sym_tab);   // delete scope for function body
    sym_tab_pop(parser_utils->stack_sym_tab);   // delete scope for paramtrs function

    if((return_code = expect_token_type(parser_utils->parser_token, TOKEN_R_CUR_BRACE))){// } 
        fprintf(stderr, "Error code:%d! Unexpected syntactic structure.\n", return_code);
        return return_code;
    };
    if(parser_utils->function->type != IFJ_VOID){
        if(!parser_utils->was_return){
            fprintf(stderr, "Error code: %d! Await return expression from not-void function \"%s\".\n", ERROR_OTHER_SEMANTICS, parser_utils->function->id->str);
            return ERROR_OTHER_SEMANTICS;
        }
    }
    parser_utils->was_return=false;
    parser_utils->in_function=false;
    return get_token(parser_utils->parser_token, parser_utils->input_code);
}



int param_list_rule(Parser_utils_t* parser_utils){
    int return_code;
    if(!expect_token_type(parser_utils->parser_token, TOKEN_RPAREN)){  // empty param list
        return OK;
    }
    if((return_code = param_rule(parser_utils, parser_utils->function->parameters->first))) return return_code;  // firts param
    return next_param_rule(parser_utils, parser_utils->function->parameters->first); // others params
}



int param_rule(Parser_utils_t* parser_utils, Parametr_list_node* param){
    int return_code;
    Data_type_IFJ new_type;
    bool flag_param_id = true;
    parser_utils->param_counter += 1;
            
    if(!expect_token_type(parser_utils->parser_token, TOKEN_IDE) || !expect_token_type(parser_utils->parser_token, TOKEN_UND)){  // param name
        if(!parser_utils->function->definition){  // pre-def
            if(param == NULL){  // more that need
                fprintf(stderr, "Error code: %d! Invalid count arguments for function with id \"%s\".\n", ERROR_BAD_PARAM_OR_BAD_RETURN_TYPE, parser_utils->function->id->str);
                return ERROR_BAD_PARAM_OR_BAD_RETURN_TYPE;
            }
            if(strcmp(parser_utils->parser_token->attribute.string->str, param->parametr_name->str)){  // incorrect param name
                fprintf(stderr, "Error code: %d! Using invalid name of parametr for function with id \"%s\".\n", ERROR_OTHER_SEMANTICS, parser_utils->function->id->str);
                return ERROR_OTHER_SEMANTICS;
            }
        }
        else{  // normal def
            if(param_name_in_param_list(parser_utils->function->parameters, parser_utils->parser_token->attribute.string)){
                fprintf(stderr, "Error code: %d! Duplicate parametrs name \"%s\" on function \"%s\".\n", ERROR_OTHER_SEMANTICS, parser_utils->parser_token->attribute.string->str, parser_utils->function->id->str);
                return ERROR_OTHER_SEMANTICS;
            }
            ds_copy(parser_utils->parser_token->attribute.string, parser_utils->param_name_aux);  // store new param_name
        }
    }
    else{  // not id for param_name
        fprintf(stderr, "Error code: %d! Unknown syntaxe structure on line.\n", ERROR_SYNTAXE);
        return ERROR_SYNTAXE;
    }

    
    if((return_code=get_token(parser_utils->parser_token, parser_utils->input_code))) return return_code;   // param_id
    if(!expect_token_type(parser_utils->parser_token, TOKEN_IDE) || !expect_token_type(parser_utils->parser_token, TOKEN_UND)){
        if(!strcmp(parser_utils->param_name_aux->str, parser_utils->parser_token->attribute.string->str) && expect_token_type(parser_utils->parser_token, TOKEN_UND)){  // name != id
            fprintf(stderr, "Error code: %d! Parametrs name and parametrs id have the same name \"%s\" on function \"%s\".\n", ERROR_OTHER_SEMANTICS, parser_utils->param_name_aux->str, parser_utils->function->id->str);
            return ERROR_OTHER_SEMANTICS;
        }
        if(expect_token_type(parser_utils->parser_token, TOKEN_UND)){
            parser_utils->var = search_on_act_scope(parser_utils->stack_sym_tab->top->tree, parser_utils->parser_token->attribute.string);  // check if alwayse exist the same param_id
            if(parser_utils->var!=NULL){  // id duplicate
                fprintf(stderr, "Error code: %d! Duplicate parametrs id \"%s\" in function \"%s\".", ERROR_OTHER_SEMANTICS, parser_utils->var->id->str, parser_utils->function->id->str);
                return ERROR_OTHER_SEMANTICS;
            }
            if((return_code = sym_tab_insert_new_id(&parser_utils->stack_sym_tab->top->tree, &parser_utils->var, parser_utils->parser_token->attribute.string, SYMBOL_CONST))) return return_code;
            parser_utils->var->definition = true;
            parser_utils->var->var_init = true;
        }
        else{
            flag_param_id=false;
        }
    }
    else{
        fprintf(stderr, "Error code: %d! Unknown syntaxe structure on line.\n", ERROR_SYNTAXE);
        return ERROR_SYNTAXE;
    }
    
    if((return_code=get_token(parser_utils->parser_token, parser_utils->input_code))) return return_code;  // : <TYPE>
    if((return_code = id_type_definition_rule(parser_utils, &(new_type), false))) return return_code;
    
    if(flag_param_id){
        parser_utils->var->type = new_type;
    }
    if(!parser_utils->function->definition){  // pre-def
        if(param->parametr_type != new_type){  // invalid type
            if(type_with_nil_compatabilty(param->parametr_type, new_type)){
                param->parametr_type = new_type;
            }
            else{
                fprintf(stderr, "Error code: %d! Unsuitable type with parametr by function \"%s\".\n", ERROR_BAD_PARAM_OR_BAD_RETURN_TYPE, parser_utils->function->id->str);
                return ERROR_BAD_PARAM_OR_BAD_RETURN_TYPE;
            }
        }
    }
    else{  // normal def
        
        if((return_code = insert_new_param_to_list(parser_utils->function->parameters, parser_utils->param_name_aux, new_type))) return return_code;  // new parametr value
    }
    return OK;
}



int next_param_rule(Parser_utils_t* parser_utils, Parametr_list_node* param){
    int return_code;
    if(expect_token_type(parser_utils->parser_token, TOKEN_COMA)){  // it was last param
            return OK;
    }
    if((return_code=get_token(parser_utils->parser_token, parser_utils->input_code))) return return_code;  // <PARAM>
    if((return_code = param_rule(parser_utils, param->next_node))) return return_code; 
    return next_param_rule(parser_utils, param->next_node);  // <NEXT_PARAM>
}



int func_type_definition_rule(Parser_utils_t* parser_utils){
    int return_code;
    Data_type_IFJ function_type;

    if(!(return_code = expect_token_type(parser_utils->parser_token, TOKEN_ARROW))){    // -> 
        if((return_code=get_token(parser_utils->parser_token, parser_utils->input_code))) return return_code;
        if((return_code = type_rule(parser_utils, &function_type))) return return_code; // <TYPE>
        if(!parser_utils->function->definition){  // pre-def
            if(parser_utils->function->type != function_type){
                if((parser_utils->function->type == IFJ_VOID) || (type_with_nil_compatabilty(function_type, parser_utils->function->type) && !parser_utils->function->maybe_type_without_nil)){
                    parser_utils->function->type = function_type;
                }
                else{  // bad type
                    fprintf(stderr, "Error code: %d! Incorrect assigment type to variable by function \"%s\".\n", ERROR_TYPE_UNCOMPATABILITY_ON_EXPR, parser_utils->function->id->str);
                    return ERROR_TYPE_UNCOMPATABILITY_ON_EXPR;
                }
            }
        }
        else{  // normal def
            parser_utils->function->type = function_type;
        }
        return_code=get_token(parser_utils->parser_token, parser_utils->input_code);
        return return_code;
    }
    else{  // e
        if(expect_token_type(parser_utils->parser_token, TOKEN_L_CUR_BRACE)){
            fprintf(stderr, "Error code: %d! Unknown syntaxe structure in position for type definiton for function \"%s\".", ERROR_SYNTAXE, parser_utils->function->id->str);
            return ERROR_SYNTAXE;
        }
        if(parser_utils->function->definition){  // normal def
            parser_utils->function->type = IFJ_VOID;
        }
        else{  // pre-def
            if(parser_utils->function->type != IFJ_VOID){
                fprintf(stderr, "Error code: %d! Incorrect assigment type to variable by function \"%s\"\n.", ERROR_TYPE_UNCOMPATABILITY_ON_EXPR, parser_utils->function->id->str);
                return ERROR_TYPE_UNCOMPATABILITY_ON_EXPR;
            }
        } 
    }
    return OK;
}



int argument_list_rule(Parser_utils_t* parser_utils){
    int return_code;
    
    if(!expect_token_type(parser_utils->parser_token, TOKEN_RPAREN)){
        if(parser_utils->function->definition || parser_utils->function->function_pre_definition){
            if(parser_utils->function->parameters->param_count > 0){
                fprintf(stderr, "Error code: %d! Invalid count of arguments in function \"%s\".", ERROR_BAD_PARAM_OR_BAD_RETURN_TYPE, parser_utils->function->id->str);
                return ERROR_BAD_PARAM_OR_BAD_RETURN_TYPE;
            }
        }
        return OK;
    }
    if((return_code = argument_rule(parser_utils, parser_utils->function->parameters->first))) return return_code;
    return next_argument_rule(parser_utils, parser_utils->function->parameters->first);
}



int argument_rule(Parser_utils_t* parser_utils, Parametr_list_node* param){
    int return_code;
    Data_type_IFJ new_type;
    bool flag_undo_name = false;
    bool flag_literal = false;
    if(is_term(parser_utils->parser_token)){  // term
        parser_utils->param_counter += 1;
        if(is_literal_value(parser_utils->parser_token)){  // literal -> param_name is _
            new_type = extract_data_type_from_token(parser_utils->parser_token);  // store type of literal
            flag_undo_name, flag_literal = true;
        }
        else  // or param_name or var
            ds_copy(parser_utils->parser_token->attribute.string, parser_utils->ds_aux);  // or param_name or term
    }
    else{
        fprintf(stderr, "Error code: %d! Unknown syntaxe structure on line.\n", ERROR_SYNTAXE);
        return ERROR_SYNTAXE;
    }

    if((return_code=get_token(parser_utils->parser_token, parser_utils->input_code))) return return_code;
    if(expect_token_type(parser_utils->parser_token, TOKEN_COL) && expect_token_type(parser_utils->parser_token, TOKEN_COMA) && expect_token_type(parser_utils->parser_token, TOKEN_RPAREN)){
        fprintf(stderr, "Error code: %d! Unknown syntaxe structure on line %d.\n", ERROR_SYNTAXE, parser_utils->parser_token->type);
        return ERROR_SYNTAXE;
    }
    if(!flag_undo_name){  // param_name = term
        if(!expect_token_type(parser_utils->parser_token, TOKEN_COL)){  // :
            ds_copy(parser_utils->ds_aux, parser_utils->param_name_aux);
            if((return_code=get_token(parser_utils->parser_token, parser_utils->input_code))) return return_code;
            if(is_term(parser_utils->parser_token)){
                if(is_literal_value(parser_utils->parser_token)){  // literal
                    flag_literal = true;
                    new_type = extract_data_type_from_token(parser_utils->parser_token);  // store type of literal
                }
                else{
                    ds_copy(parser_utils->parser_token->attribute.string, parser_utils->ds_aux);  // store name of var
                }
            }
            else{
                fprintf(stderr, "Error code: %d! Unknown syntaxe structure on line.\n", ERROR_SYNTAXE);
                return ERROR_SYNTAXE;
            }
            if((return_code=get_token(parser_utils->parser_token, parser_utils->input_code))) return return_code;
               
        }
        else{
            flag_undo_name = true;
        }    
    }    
    if(parser_utils->function->definition || parser_utils->function->function_pre_definition){  // normal def
        if(param == NULL){
            fprintf(stderr, "Error code: %d! Invalid parametrs count for function with id \"%s\".\n", ERROR_BAD_PARAM_OR_BAD_RETURN_TYPE, parser_utils->function->id->str);
            return ERROR_BAD_PARAM_OR_BAD_RETURN_TYPE;
        }
        if(flag_undo_name){
            if((strcmp(param->parametr_name->str, UNDO))){
                fprintf(stderr, "Error code: %d! Invalid parametrs name for function \"%s\".", ERROR_OTHER_SEMANTICS, parser_utils->function->id->str);
                return ERROR_OTHER_SEMANTICS;
            } 
        }
        if(!flag_undo_name){
            if(strcmp(param->parametr_name->str, parser_utils->param_name_aux->str)){
                fprintf(stderr, "Error code: %d! Invalid parametrs name for function \"%s\".", ERROR_BAD_PARAM_OR_BAD_RETURN_TYPE, parser_utils->function->id->str);
                return ERROR_OTHER_SEMANTICS;
            }
        }
        if(!flag_literal){
            if((return_code = check_is_var(parser_utils, parser_utils->ds_aux))) return return_code;
            new_type = parser_utils->var->type;
        }
        if((return_code = args_type_compatability(parser_utils->function, new_type, &(param->parametr_type)))) return return_code;
        
    }
    else{    // post def
        if(flag_undo_name){
            ds_free(&(parser_utils->param_name_aux));
            ds_init(&(parser_utils->param_name_aux));
            ds_add_chars(parser_utils->param_name_aux, UNDO);
        }
        if(!flag_literal){
            if((return_code = check_is_var(parser_utils, parser_utils->ds_aux))) return return_code;
            new_type = parser_utils->var->type;
        }
        if((return_code = insert_new_param_to_list(parser_utils->function->parameters, parser_utils->param_name_aux, new_type))) return return_code;  // new parametr value
    }

    return OK;
}


int next_argument_rule(Parser_utils_t* parser_utils, Parametr_list_node* param){
    int return_code;
    if(expect_token_type(parser_utils->parser_token, TOKEN_COMA)){  // unsupported synt str
            return OK;
    }
    if((return_code=get_token(parser_utils->parser_token, parser_utils->input_code))) return return_code;
    if((return_code = argument_rule(parser_utils, param->next_node))) return return_code; 
    return next_argument_rule(parser_utils, param->next_node);
}



int call_function_rule(Parser_utils_t* parser_utils, bool assigm_opt){
    int return_code;
    Data_type_IFJ new_type;
    Symbol_t* in_wich_func_body = parser_utils->function;
    bool first_func_read=false;
    
    
    parser_utils->function = search_on_all_scope(parser_utils->stack_sym_tab->top, parser_utils->ds_aux);
    
    if(parser_utils->function==NULL){  // unknown func name
        first_func_read=true;
        if((return_code = sym_tab_insert_new_id(parser_utils->global_sym_tab, &parser_utils->function, parser_utils->ds_aux, SYMBOL_FUNCTION))) return return_code;
    
    }
    
    if(parser_utils->function->sym_class!=SYMBOL_FUNCTION){
        fprintf(stderr, "Error code:%d! Using undefenited function \"%s\".\n", ERROR_UNDEF_FUNC_OR_REDEF_VAR, parser_utils->function->id->str);
        return ERROR_UNDEF_FUNC_OR_REDEF_VAR;
    }
    
    parser_utils->param_counter=0;
    
    if((return_code=get_token(parser_utils->parser_token, parser_utils->input_code))) return return_code;
    
    if(!strcmp(parser_utils->function->id->str, FOO_WRITE)){   // write()
            if((return_code = write_arg_list_rule(parser_utils))) return return_code;
    }
    else{  // other foo()
        if((return_code=argument_list_rule(parser_utils))) return return_code;  // <ARG_LIST>
    }
        
    if((return_code = expect_token_type(parser_utils->parser_token, TOKEN_RPAREN))){ // )
    
        fprintf(stderr, "Error code:%d! Unexpected syntactic structure on argumets list when call function \"%s\".\n", ERROR_SYNTAXE, parser_utils->function->id->str);
        return ERROR_SYNTAXE;
    }
    
    if((return_code = check_param_count(parser_utils))) return return_code;
    
    if(assigm_opt){  // id = func()
        if((return_code = func_call_type_compatability(parser_utils->function, parser_utils->lhs))) return return_code;
    }
    else{  // func()
        if(first_func_read){
            parser_utils->function->type=IFJ_VOID;  //unknown returned type
        }
    }    
    if(!parser_utils->function->definition&& !parser_utils->function->function_pre_definition){
        parser_utils->function->function_pre_definition=true;
    }
    parser_utils->function = in_wich_func_body;      
    if((return_code=get_token(parser_utils->parser_token, parser_utils->input_code))) return return_code;
    return OK;
}

int check_param_count(Parser_utils_t* parser_utils){
    if(!strcmp(parser_utils->function->id->str, FOO_WRITE)){
        if(parser_utils->param_counter < 1){
            fprintf(stderr, "Error code: %d! Incorrect count of arguments with function \"write\".\n", ERROR_BAD_PARAM_OR_BAD_RETURN_TYPE);
            return ERROR_BAD_PARAM_OR_BAD_RETURN_TYPE;    
        }
    }
    else if(parser_utils->function->parameters->param_count != parser_utils->param_counter){
        fprintf(stderr, "Error code: %d! Incorrect count of arguments with function \"%s\".\n", ERROR_BAD_PARAM_OR_BAD_RETURN_TYPE, parser_utils->function->id->str);
        return ERROR_BAD_PARAM_OR_BAD_RETURN_TYPE;
    }
    parser_utils->param_counter=0;
    return OK;
}

int return_rule(Parser_utils_t* parser_utils){
    int return_code;
    
    parser_utils->function->function_return = true;
    if(!parser_utils->in_function){  // return only in function
        fprintf(stderr, "Error code: %d! Using \"return\" instreuction not in function body.\n", ERROR_SYNTAXE);
        return ERROR_SYNTAXE;
    }
    if((return_code=get_token(parser_utils->parser_token, parser_utils->input_code))) return return_code;
    
    if((return_code = rule_expression(parser_utils, parser_utils->input_code))) return return_code;
    parser_utils->function->function_return = false;
    parser_utils->was_return=true;
    return OK;
}

int write_arg_list_rule(Parser_utils_t* parser_utils){
    int return_code;
    if(!expect_token_type(parser_utils->parser_token, TOKEN_RPAREN)){
        return OK;
    }
    if((return_code = write_arg(parser_utils))) return return_code;
    return write_next_arg(parser_utils);
}
int write_arg(Parser_utils_t* parser_utils){
    int return_code;
    parser_utils->param_counter += 1;
            
    if(is_term(parser_utils->parser_token)){
        
        if(is_literal_value(parser_utils->parser_token)){
            // literal for gen
            
        }
        else{
            if((return_code = check_is_var(parser_utils, parser_utils->parser_token->attribute.string))) return return_code;
        }
        return get_token(parser_utils->parser_token, parser_utils->input_code);       
    }
    fprintf(stderr, "Error code: %d! Unknown syntaxe structure on line.\n", ERROR_SYNTAXE);
    return ERROR_SYNTAXE;
}
int write_next_arg(Parser_utils_t* parser_utils){
    int return_code;
    if(expect_token_type(parser_utils->parser_token, TOKEN_COMA)){  // it was last param
        return OK;
    }
    if((return_code=get_token(parser_utils->parser_token, parser_utils->input_code))) return return_code;  // <PARAM>
    if((return_code = write_arg(parser_utils))) return return_code; 
    return write_next_arg(parser_utils);
}
//================================================
