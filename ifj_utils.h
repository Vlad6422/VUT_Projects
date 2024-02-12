// Author: Myron Kukhta (xkukht01)
// project: IFJ2023 imperative language translator implementation
#ifndef IFJ_UTILS_H
#define IFJ_UTILS_H


// returned value

// returned type
#define SCAN_END 0
#define OK 0
#define ERROR_LEX 1
#define ERROR_SYNTAXE 2
#define ERROR_UNDEF_FUNC_OR_REDEF_VAR 3
#define ERROR_BAD_PARAM_OR_BAD_RETURN_TYPE 4
#define ERROR_UNDEF_OR_UNINIT_VAR 5
#define ERROR_BAD_EXPRESSION_ON_RETURN 6
#define ERROR_TYPE_UNCOMPATABILITY_ON_EXPR 7
#define ERROR_UNKNOWN_TYPE_VAR_OR_PARAM 8
#define ERROR_OTHER_SEMANTICS 9
#define ERROR_INTERNAL 99

// aux string for func name
#define UNDO "_"
//aux string for name func write()
#define FOO_WRITE "write"
//aux string for name func readString()
#define FOO_READ_STRING "readString"
//aux string for name func readInt()
#define FOO_READ_INT "readInt"
//aux string for name func readDouble()
#define FOO_READ_DOUBLE "readDouble"
//aux string for name func Int2Double()
#define FOO_INT_TO_DOUBLE "Int2Double"
//aux string for name func Double2Int()
#define FOO_DOUBLE_TO_INT "Double2Int"
//aux string for name func length()
#define FOO_LENGTH "length"
//aux string for name func substring()
#define FOO_SUBSTRING "substring"
//aux string for name func ord()
#define FOO_ORD "ord"
//aux string for name func chr()
#define FOO_CHR "chr"

#define OF "of"
#define START_AT "startingAt"
#define END_BEFOUR "endingBefore"

// data type in ifj language
typedef enum{
    IFJ_NOT_SET_TYPE,
    IFJ_INT,
    IFJ_INT_NIL,
    IFJ_DOUB,
    IFJ_DOUB_NIL,
    IFJ_STR,
    IFJ_STR_NIL,
    IFJ_VOID,
    IFJ_NIL
}Data_type_IFJ;
#endif