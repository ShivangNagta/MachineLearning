import numpy as np
import matplotlib.pyplot as plt

fx = lambda x : x**2 + 3*x
dfx = lambda x : 2*x + 3

x = np.linspace(-4,2,2001)

learningRate = 0.001
epochs = 10000

startx = np.random.choice(x,1)

for i in range(epochs):
    grad = dfx(startx)
    startx = startx - learningRate*grad
    
print(startx)


plt.plot(x,fx(x), x,dfx(x))
plt.plot(startx,fx(startx),'bo')
plt.plot(startx,dfx(startx),'bo')

plt.xlim(x[[0,-1]])
plt.grid()
plt.xlabel('x')
plt.ylabel('f(x)')
plt.legend(['y','dy'])
plt.show()