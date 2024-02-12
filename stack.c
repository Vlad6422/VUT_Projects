// Author: Vladyslav Malashchuk (xmalas04)
// project: IFJ2023 imperative language translator implementation
#include "stack.h"

void s_init ( Stack *stack ) {

    stack->top = NULL;

}

void s_push ( Stack *stack, Data_type type, pt_symbol symbol) {

    Stack_item *newItem = malloc( sizeof ( Stack_item ) );
    if (!newItem) exit(ERROR_INTERNAL);

    newItem->nextItem = stack->top;
    newItem->symbol = symbol;
    newItem->type = type;
    stack->top = newItem;
    
}

void s_push_before_terminal ( Stack *stack, Data_type type, pt_symbol symbol ) {

    Stack_item *tmp1 = stack->top;
    Stack_item *tmp2 = NULL;

    while (1) {

        if (!tmp1) break;

        if (tmp1->symbol != NON_TERM && tmp1->symbol != STOP) {

            Stack_item *newItem = malloc( sizeof( Stack_item ) );
            if (!newItem) return;
            
            newItem->symbol = symbol;
            newItem->type = type;
            
            if (!tmp2) {
                newItem->nextItem = stack->top;
                stack->top = newItem;
            }

            else {
                newItem->nextItem = tmp1;
                tmp2->nextItem = newItem;
            }

            return;

        }

        tmp2 = tmp1;
        tmp1 = tmp1->nextItem;

    }

}

void s_pop ( Stack *stack ) {

    if (stack->top != NULL) {
        Stack_item *toPop = stack->top;
        stack->top = stack->top->nextItem;
        free( toPop );
    }

}

Stack_item *s_top ( Stack *stack ) {

    return stack->top;

}

Data_type s_top_type ( Stack *stack ) {

    return stack->top->type;

}

pt_symbol s_top_terminal_symbol ( Stack *stack ) {

    Stack_item *topTerminal = stack->top;
    
    while (topTerminal) {

        if (topTerminal->symbol != NON_TERM && topTerminal->symbol != STOP) break;
        topTerminal = topTerminal->nextItem;

    }

    return topTerminal->symbol;

}

void s_dispose ( Stack *stack ) {

    while (stack->top) {
        
        s_pop( stack );       

    }

}