: 2DROP ( x y -- ) DROP DROP ;
: 3DROP ( x y z -- ) 2DROP DROP ;
: 4DROP ( x y z q -- ) 3DROP DROP ;
: NIP ( x y -- y ) SWAP DROP ;
: TUCK ( x y -- y x y ) SWAP OVER ;

: TYPE ( c-addr +n -- )
    ?DUP IF
        OVER + SWAP DO I C@ EMIT LOOP
    ELSE DROP THEN ;

: CR ( -- ) 13 EMIT 10 EMIT ;

: ABORT ( -- ) RECURSE ;
