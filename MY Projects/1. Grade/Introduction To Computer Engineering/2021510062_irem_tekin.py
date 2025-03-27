operations = ["**", "*", "//", "/", "%", "+", "-", "<", ">", "==", "!=", "!"]
sayilar = ["0", "1", "2", "3", "4", "5", "6", "7", "8", "9"]
operationsPart = ["!", "="]
space = [" ", "\n"]


def function():
    with open("input.txt", encoding='utf-8') as file: #read the input file.

     for line in file: #allocate every line in the txt file.
        if line.startswith("\n"):
            f.write("\n")
        else:
            space2 = True
            for x in line:
                if ( (x == " " or x == "\n")):
                    operaators(line)
                    space2 = True
                    break
            if (space2):
                f.write("")


def factorial(num):#I calculated the factorial in case it is used.
    result = 1
    for x in range(1, num + 1):
        result = result * x
    return result


def operaators(expr):#the function gives error
    elements = []
    temporary = ""
    sayilarFlag = False
    operationFlag = False
    ErrorFlag = False
    for x in expr:

        if x in sayilar:
            if (operationFlag):
                elements.append(temporary)
                temporary = ""
                operationFlag = False
            temporary = temporary + x
            sayilarFlag = True
        elif x in operations or x in operationsPart:
            if (sayilarFlag):
                elements.append(int(temporary))
                temporary = ""
                sayilarFlag = False
            temporary = temporary + x
            operationFlag = True
        elif x in space:
            if (operationFlag):
                elements.append(temporary)
                temporary = ""
                operationFlag = False
            if (sayilarFlag):
                elements.append(int(temporary))
                temporary = ""
                sayilarFlag = False

        else:
            ErrorFlag = True
            break

    if (ErrorFlag):
        f.write("ERROR\n")
    else:
        for x in elements:
            if (not (x in operations or isinstance(x, int))):
                f.write("ERROR\n")
                return -1
        try:
            evaluate(elements)
        except:
            f.write("ERROR\n")


def evaluate(a):
    if ("**" in a):#calculating power
        z = a.index("**")
        result = a[z - 1] ** a[z + 1]
        a.pop(z + 1)
        a.pop(z)
        a[z - 1] = result


    elif ("*" in a):#calculating multiplation
        z = a.index("*")
        result = a[z - 1] * a[z + 1]
        a.pop(z + 1)
        a.pop(z)
        a[z - 1] = result


    elif ("//" in a):#calculating integer division
        z = a.index("//")
        result = a[z - 1] // a[z + 1]
        if (a[z + 1] == 0):
            f.write("ERROR\n")
            return -1
        a.pop(z + 1)
        a.pop(z)
        a[z - 1] = result


    elif ("/" in a):#calculating division
        z = a.index("/")
        if (a[z + 1] == 0):
            f.write("ERROR\n")
            return -1
        result = a[z - 1] / a[z + 1]
        a.pop(z + 1)
        a.pop(z)
        a[z - 1] = result


    elif ("%" in a):#calculating modulus
        z = a.index("%")
        result = a[z - 1] % a[z + 1]
        a.pop(z + 1)
        a.pop(z)
        a[z - 1] = result


    elif ("+" in a):#calculating addition
        z = a.index("+")
        result = a[z - 1] + a[z + 1]
        a.pop(z + 1)
        a.pop(z)
        a[z - 1] = result


    elif ("-" in a):#calculating subtraction
        z = a.index("-")
        result = a[z - 1] - a[z + 1]
        a.pop(z + 1)
        a.pop(z)
        a[z - 1] = result


    elif (">" in a):#Find the grater than
        z = a.index(">")
        result = a[z - 1] > a[z + 1]
        a.pop(z + 1)
        a.pop(z)
        a[z - 1] = result


    elif ("==" in a):#equal
        z = a.index("==")
        result = a[z - 1] == a[z + 1]
        a.pop(z + 1)
        a.pop(z)
        a[z - 1] = result


    elif ("<" in a):#Find the less than

        z = a.index("<")
        result = a[z - 1] < a[z + 1]
        a.pop(z + 1)
        a.pop(z)
        a[z - 1] = result


    elif ("!=" in a):#not equal
        z = a.index("!=")
        result = a[z - 1] != a[z + 1]
        a.pop(z + 1)
        a.pop(z)
        a[z - 1] = result


    if (len(a) > 1):
        evaluate(a)
    else:
        f.write(str(a[0]) + "\n")


f = open("2021510062_irem_tekin_output.txt", 'a')#the output is given as txt
function()
