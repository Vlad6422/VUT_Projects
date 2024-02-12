// Author: Vladyslav Malashchuk (xmalas04)
// project: IFJ2023 imperative language translator implementation
#ifndef STACK_H
#define STACK_H

#include "scanner.h"
#include "expression.h"

#include <stdbool.h>
#include <stdlib.h>

// Stack item
typedef struct SElem {

    pt_symbol symbol;
    Data_type type;
    struct SElem *nextItem;

} Stack_item;
// Structure of stack for expression processing
typedef struct {
    
    Stack_item *top;

} Stack;

/**
 * @brief Initializes the stack
 */
void s_init ( Stack *stack );

/**
 * @brief Pushes item into stack
 * @param stack stack for expression processing
 * @param data stack item data type
 * @param symbol stack item symbol
 */
void s_push ( Stack *stack, Data_type data, pt_symbol symbol);

/**
 * @brief Pushes item into stack before first occured terminal
 * @param stack stack for expression processing
 * @param data stack item data type
 * @param symbol stack item symbol
 */
void s_push_before_terminal ( Stack *stack, Data_type data, pt_symbol symbol );

/**
 * @brief Discards one stack item
 * @param stack stack for expression processing
 */
void s_pop ( Stack *stack );

/**
 * @brief Returns stack's top item
 * @param stack stack for expression processing
 * @return Item on the top of the stack
 */
Stack_item *s_top ( Stack *stack );

/**
 * @brief Returns top item's data type
 * @param stack stack for expression processing
 * @return Data type
 */
Data_type s_top_type ( Stack *stack );

/**
 * @brief Returns first occured terminal symbol
 * @param stack stack for expression processing
 * @return Precedence table symbol
 */
pt_symbol s_top_terminal_symbol ( Stack *stack );

/**
 * @brief Discards all remaining stack items
 * @param stack stack for expression processing
 */ 
void s_dispose ( Stack *stack );   

#endif