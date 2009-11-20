all: samc test.rom

samc: samc.c
	gcc -g samc.c -o samc -std=c99

test.rom: test.asm
	~/builds/sjasm42b8/sjasm -j test.asm test.rom

test.asm: test.fs samc
	./samc test.fs > test.asm

run: test.rom
	openmsx -cart test.rom

clean:	
	rm -f test.asm test.lst test.rom samc

