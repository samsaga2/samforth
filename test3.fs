INCLUDE kernel.fs
INCLUDE bios.fs

: test-exit-loop ( -- )
    10 0 do
        i . cr
        i 3 = if
            unloop exit
        then
    loop ;

: show-keys ( -- )
    CHGET . cr recurse ;

: main
    decimal cr
    ." Hello world!" cr
    test-exit-loop
    show-keys ;
