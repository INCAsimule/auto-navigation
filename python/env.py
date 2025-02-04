import argparse
from dataclasses import dataclass

from mlagents_envs.environment import UnityEnvironment
from mlagents_envs.envs.unity_gym_env import UnityToGymWrapper
from gymnasium import Env, spaces
import numpy as np

class BoatEnv(Env):
    def __init__(self):
        super(BoatEnv, self).__init__()
        
        # Définir l'espace d'action
        self.action_space = spaces.Discrete(4)  # 4 actions possibles
        
        # Définir l'espace d'observation
        self.observation_space = spaces.Box(
            low=np.array([0, 0]), 
            high=np.array([10, 10]),
            dtype=np.float32
        )
        
        self.state = None
        self.max_steps = 100
        self.current_step = 0

    def reset(self, seed=None):
        super().reset(seed=seed)
        self.current_step = 0
        self.state = np.array([0, 0])
        return self.state, {}

    def step(self, action):
        self.current_step += 1
        
        # Appliquer l'action
        if action == 0:  # haut
            self.state[1] += 1
        elif action == 1:  # bas
            self.state[1] -= 1
        elif action == 2:  # droite
            self.state[0] += 1
        elif action == 3:  # gauche
            self.state[0] -= 1
            
        # Calculer la récompense
        reward = -1  # Pénalité par défaut
        done = False
        
        # Vérifier si but atteint
        if self.state[0] == 5 and self.state[1] == 5:
            reward = 100
            done = True
            
        # Vérifier si hors limites
        if (self.state < 0).any() or (self.state > 10).any():
            reward = -100
            done = True
            
        # Vérifier nombre max d'étapes
        if self.current_step >= self.max_steps:
            done = True
            
        return self.state, reward, done, False, {}

    def render(self):
        # Optionnel: afficher l'état actuel
        print(f"Position: {self.state}")


def main():

    # Se connecter à l'environnement de débogage dans l'éditeur Unity
    env = UnityEnvironment(file_name=None, seed=1, side_channels=[])
    print("Environment created")
    env.reset()

    # Récupérer le nom de comportement (behavior)
    behavior_name = list(env.behavior_specs.keys())[0]
    print(f"Behavior name: {behavior_name}")
    spec = env.behavior_specs[behavior_name]
    print(f"Observation shape: {spec}")

if __name__ == "__main__":
    main()
    