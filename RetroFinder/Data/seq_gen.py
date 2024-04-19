import random


def mutate(seq):
    result = ""
    for char in seq:
        chance = random.randint(0, 99)
        if chance > 70:
            result += random.choices(alphabet)[0]
        else:
            result += char
    return result


seqs = {}
current_domain = ''

with open('protein_domains.fa', 'r') as file:
    for line in file:
        if line[0] == '>':
            seqs[line.split('>')[1].rstrip()] = ''
            current_domain = line.split('>')[1].rstrip()
        else:
            seqs[current_domain] = line.rstrip()


alphabet = "ACGT"
ltr = "".join(random.choices(alphabet, k = 222))
ltr2 = "".join(random.choices(alphabet, k = 206))
with open("test_sequence.fa", 'w') as file:
    file.write(">test sequence 1\n")
    file.write("".join(random.choices(alphabet, k = 23)))
    file.write("N" * 42)
    file.write("".join(random.choices(alphabet, k = 66)))
    file.write(ltr)
    file.write("".join(random.choices(alphabet, k = 16)))
    file.write(mutate(seqs["GAG " + str(random.randint(1, 3))]))
    file.write("".join(random.choices(alphabet, k = 16)))
    file.write(mutate(seqs["PROT " + str(random.randint(1, 3))]))
    file.write("".join(random.choices(alphabet, k = 16)))
    file.write(mutate(seqs["INT " + str(random.randint(1, 3))]))
    file.write("".join(random.choices(alphabet, k = 16)))
    file.write(mutate(seqs["RT " + str(random.randint(1, 3))]))
    file.write("".join(random.choices(alphabet, k = 16)))
    file.write(mutate(seqs["RH " + str(random.randint(1, 3))]))
    file.write("".join(random.choices(alphabet, k = 16)))
    file.write(ltr)
    file.write("".join(random.choices(alphabet, k = 66)))
    file.write(ltr2)
    file.write("".join(random.choices(alphabet, k = 16)))
    file.write(mutate(seqs["GAG " + str(random.randint(1, 3))]))
    file.write("".join(random.choices(alphabet, k = 16)))
    file.write("".join(random.choices(alphabet, k = 16)))
    file.write(mutate(seqs["RT " + str(random.randint(1, 3))]))
    file.write("".join(random.choices(alphabet, k = 16)))
    file.write(mutate(seqs["RH " + str(random.randint(1, 3))]))
    file.write("".join(random.choices(alphabet, k = 16)))
    file.write(ltr2)


    
               
                       
