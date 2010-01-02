INCLUDE kernel.fs
INCLUDE bios.fs

: test-exit-loop ( -- )
    10 0 do
        i . cr
        i 6 = if
            unloop exit
        then
    2 +loop ;

: show-keys ( -- )
    CHGET . cr recurse ;

: main
    100 200 300 .s
    decimal cr
    ." Hello world!" cr
    test-exit-loop
    show-keys ;

